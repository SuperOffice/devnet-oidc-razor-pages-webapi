using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;
using SuperOffice.DevNet.Asp.Net.RazorPages.Middleware;
using System;
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
            logger.LogInformation("RefreshTokenMiddleware: entered Invoke.");

            // only process authenticated users, i.e. has stored tokens

            AuthenticateResult authenticateResult = await context.AuthenticateAsync();

            if (authenticateResult != null && authenticateResult.Succeeded)
            {
                logger.LogInformation("RefreshTokenMiddleware.");

                // determine if the access token is expired

                var expiresAt = authenticateResult.Properties.GetTokenValue("expires_at");

                // some providers do not set expires_at or refresh_token, i.e. twitter.

                if (expiresAt != null)
                {
                    DateTime accessTokenExpiresAt = DateTime.Parse(expiresAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

                    if (accessTokenExpiresAt <= DateTime.UtcNow.AddMinutes(-5))
                    {
                        await RefreshAccessTokenAsync(context, authenticateResult);
                    }
                }
                else
                {
                    logger.LogInformation("RefreshTokenMiddleware: {0} provider has not stored expired_at token.", authenticateResult.Principal.Identity.AuthenticationType);
                }
            }

            logger.LogInformation("RefreshTokenMiddleware: calling next(context).");

            await next(context);

            logger.LogInformation("RefreshTokenMiddleware: leaving Invoke.");
        }

        private async Task RefreshAccessTokenAsync(HttpContext context, AuthenticateResult authResult)
        {
            logger.LogInformation("RefreshTokenMiddleware: entered RefreshAccessTokenAsync.");

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

            logger.LogInformation("RefreshTokenMiddleware: requesting new tokens.");

            var tokenResponse = await authOptions.Options.Backchannel.PostAsync(
                authOptions.TokenEndpoint, encodedContent, context.RequestAborted);

            if (tokenResponse.IsSuccessStatusCode)
            {
                var responseStream = await tokenResponse.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseStream);
                
                string accessToken = json[OpenIdConnectParameterNames.AccessToken].Value<string>();
                string idToken     = json[OpenIdConnectParameterNames.IdToken].Value<string>();
                string tokenType   = json[OpenIdConnectParameterNames.TokenType].Value<string>();
                int expiresIn      = json[OpenIdConnectParameterNames.ExpiresIn].Value<int>();

                var expiresAt = (DateTimeOffset.UtcNow + TimeSpan.FromSeconds(expiresIn)).ToString("o", CultureInfo.InvariantCulture);

                logger.LogInformation("RefreshTokenMiddleware: new access token received.\r\n{0}", accessToken);

                properties.UpdateTokenValue(OpenIdConnectParameterNames.AccessToken, accessToken);
                properties.UpdateTokenValue(OpenIdConnectParameterNames.IdToken, idToken);
                properties.UpdateTokenValue(OpenIdConnectParameterNames.TokenType, tokenType);
                properties.UpdateTokenValue("expires_at", expiresAt);

                await context.SignInAsync(authResult.Principal, properties);
            }

            logger.LogInformation("RefreshTokenMiddleware: entered RefreshAccessTokenAsync.");

            return;
        }

        private async Task<MiddlewareAuthenticationOptions> GetSettingsAsync(HttpContext context, string authScheme)
        {
            logger.LogInformation("RefreshTokenMiddleware: entered GetSettingsAsync.");

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

                    logger.LogInformation("RefreshTokenMiddleware: leaving GetSettingsAsync with OIDC settings.");

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
                    // give oAuthOptions a try...
                    logger.LogError("RefreshTokenMiddleware: GetSettingsAsync failed using OIDC settings.");
                }
            }

            try
            {
                oAuthOptions = this.oAuthOptions as OAuthOptions;
                if (oAuthOptions != null)
                {
                    logger.LogInformation("RefreshTokenMiddleware: leaving GetSettingsAsync with OAuth settings.");

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
                logger.LogError("RefreshTokenMiddleware: GetSettingsAsync failed to get any OIDC or OAuth settings.");

                throw new InvalidOperationException($"Unable to load RemoteAuthenticationOptions configuration for configured scheme: {e.Message}", e);
            }


            throw new InvalidOperationException("Unable to determine RemoteAuthenticationOptions.");
        }
    }
}
