using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Newtonsoft.Json.Linq;
using SuperOffice.DevNet.Asp.Net.RazorPages.Middleware;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public class RefreshTokenMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RefreshTokenMiddleware> logger;
        private readonly IAuthenticationService authenticationService;
        private readonly IAuthenticationSchemeProvider schemeProvider;
        private readonly IOptionsMonitor<OpenIdConnectOptions> authOptions;
        private readonly IOptionsMonitor<OAuthOptions> oAuthOptions;

        public RefreshTokenMiddleware(
            RequestDelegate next,
            ILogger<RefreshTokenMiddleware> logger,
            IAuthenticationSchemeProvider schemeProvider,
            IOptionsMonitor<OpenIdConnectOptions> options,
            IOptionsMonitor<OAuthOptions> oAuthOptions)
        {
            this.next = next;
            this.logger = logger;
            this.schemeProvider = schemeProvider;
            this.authOptions = options;
            this.oAuthOptions = oAuthOptions;
        }

        public async Task Invoke(HttpContext context)
        {
            // call next first, to establish the User principal.

            await next(context);

            // process only is user is authenticated, 
            // i.e. has authenticated and tokens have been stored.

            AuthenticateResult authenticateResult = await context.AuthenticateAsync();

            if (authenticateResult != null && authenticateResult.Succeeded)
            {
                AuthenticationProperties properties = authenticateResult.Properties;

                // determine if the access token is expired

                var expiresAt = properties.GetTokenValue("expires_at");

                if (expiresAt == null)
                {
                    // authentication provider did not supply tokens to renew.
                    return;
                }

                expiresAt = properties.Items.Where(kv => kv.Key.Contains("expires", StringComparison.OrdinalIgnoreCase)).Select(kv => kv.Value).FirstOrDefault();
                DateTime accessTokenExpiresAt = DateTime.Parse(expiresAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

                if (accessTokenExpiresAt <= DateTime.UtcNow)
                {
                    await RefreshAccessTokenAsync(context, authenticateResult);
                }
            }
        }

        private async Task RefreshAccessTokenAsync(HttpContext context, AuthenticateResult authResult)
        {
            var properties = authResult.Properties;
            var authOptions = await GetSettingsAsync(context, authResult.Principal.Identity.AuthenticationType);

            var refreshToken = properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);

            if (string.IsNullOrEmpty(refreshToken))
            {
                await context.ChallengeAsync();
            }

            var parameters = new System.Collections.Generic.Dictionary<string, string> {
                { OpenIdConnectParameterNames.ClientId, authOptions.ClientId },
                { OpenIdConnectParameterNames.ClientSecret, authOptions.ClientSecret },
                { OpenIdConnectParameterNames.RefreshToken, refreshToken },
                { OpenIdConnectParameterNames.RedirectUri, authOptions.CallbackPath},
                { OpenIdConnectParameterNames.GrantType, OpenIdConnectParameterNames.RefreshToken }
            };

            var encodedContent = new FormUrlEncodedContent(parameters);


            var tokenResponse = await authOptions.Options.Backchannel.PostAsync(
                authOptions.TokenEndpoint, encodedContent, context.RequestAborted);

            if (tokenResponse.IsSuccessStatusCode)
            {
                var responseStream = await tokenResponse.Content.ReadAsStringAsync();

                #region System.Text.Json Alternative
                //var payLoad = System.Text.Json.JsonDocument
                //props.UpdateTokenValue("access_token", payload.RootElement.GetString("access_token"));
                //props.UpdateTokenValue("refresh_token", payload.RootElement.GetString("refresh_token"));
                //if (payload.RootElement.TryGetProperty("expires_in", out var property) && property.TryGetInt32(out var seconds))
                //{
                //    var expiresAt = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(seconds);
                //    props.UpdateTokenValue("expires_at", expiresAt.ToString("o", CultureInfo.InvariantCulture));
                //}.Parse(responseStream); 
                #endregion

                var json = JObject.Parse(responseStream);
                string accessToken = json[OpenIdConnectParameterNames.AccessToken].Value<string>();
                string idToken = json[OpenIdConnectParameterNames.IdToken].Value<string>();
                string tokenType = json[OpenIdConnectParameterNames.TokenType].Value<string>();
                int expiresIn = json[OpenIdConnectParameterNames.ExpiresIn].Value<int>();

                var expiresAt = (DateTimeOffset.UtcNow + TimeSpan.FromSeconds(expiresIn)).ToString("o", CultureInfo.InvariantCulture);

                properties.UpdateTokenValue(OpenIdConnectParameterNames.AccessToken, accessToken);
                properties.UpdateTokenValue(OpenIdConnectParameterNames.IdToken, idToken);
                properties.UpdateTokenValue(OpenIdConnectParameterNames.TokenType, tokenType);
                properties.UpdateTokenValue("expires_at", expiresAt);

                await context.AuthenticateAsync(authResult.Ticket.AuthenticationScheme);
                await context.SignInAsync(context.User, properties);
            }

            return;
        }

        private async Task<MiddlewareAuthenticationOptions> GetSettingsAsync(HttpContext context, string authScheme)
        {
            OpenIdConnectOptions oidcOptions;
            OAuthOptions oAuthOptions;

            var schemes = await schemeProvider.GetAllSchemesAsync();
            var scheme = schemes.Where(s => s.Name == authScheme).FirstOrDefault();

            if (scheme is null)
            {
                throw new InvalidOperationException($"No authentication scheme configured for {authScheme} to get client configuration.");
            }

            oidcOptions = authOptions.Get(scheme.Name) as OpenIdConnectOptions;
            if (oidcOptions != null)
            {
                OpenIdConnectConfiguration configuration;
                try
                {
                    configuration = await oidcOptions.ConfigurationManager.GetConfigurationAsync(context.RequestAborted);
                    return new MiddlewareAuthenticationOptions
                    {
                        ClientId = oidcOptions.ClientId,
                        ClientSecret = oidcOptions.ClientSecret,
                        CallbackPath = oidcOptions.CallbackPath,
                        TokenEndpoint = configuration.TokenEndpoint,
                        Options = oidcOptions
                    };
                }
                catch (Exception e)
                {
                    // git oAuthOptions a try...

                    // throw new InvalidOperationException($"Unable to load OpenID configuration for configured scheme: {e.Message}");
                }
            }

            try
            {
                oAuthOptions = this.oAuthOptions as OAuthOptions;
                if (oAuthOptions != null)
                {
                    return new MiddlewareAuthenticationOptions
                    {
                        ClientId = oAuthOptions.ClientId,
                        ClientSecret = oAuthOptions.ClientSecret,
                        CallbackPath = oAuthOptions.CallbackPath,
                        TokenEndpoint = oAuthOptions.TokenEndpoint,
                        Options = oAuthOptions
                    };
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to load RemoteAuthenticationOptions configuration for configured scheme: {e.Message}", e);
            }


            throw new InvalidOperationException("Unable to determine RemoteAuthenticationOptions.");
        }
    }
}
