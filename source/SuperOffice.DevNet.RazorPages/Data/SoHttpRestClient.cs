using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Dynamic;
using Microsoft.AspNetCore.Identity;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Data
{
    public interface IHttpRestClient
    {
        Task<string> Delete(string path, string fullpath = null);
        Task<string> Get(string path, string fullpath = null, StringDictionary extraHeaders = null, StringDictionary returnHeaders = null, string acceptContentType = null);
        Task<string> Patch(string path, HttpContent content, string fullpath = null, StringDictionary returnHeaders = null, StringDictionary extraHeaders = null);
        Task<string> Post(string path, HttpContent content, string fullpath = null, StringDictionary extraHeaders = null, StringDictionary returnHeaders = null);
        Task<string> Put(string path, HttpContent content, string fullpath = null, StringDictionary extraHeaders = null, StringDictionary returnHeaders = null);
    }

    public class SoHttpRestClient : IHttpRestClient
    {
        public static string IntegratedUser = "*INTEGRATED*";
        
        private IAuthenticationSchemeProvider         _schemeProvider;
        private IHttpClientFactory                    _httpClientFactory;
        private IOptionsMonitor<OpenIdConnectOptions> _oidcOptions;
        private HttpClient  _client;
        private HttpContext _context;
        private string      _defaultUserName = "Tony";
        private string      _defaultPassword = "";
        private string      _defaultUrl      = "http://localhost:3000/";
        private string      _tenantWebApiUrl;

        
        public SoHttpRestClient(
            IHttpContextAccessor context, 
            IHttpClientFactory httpClientFactory, 
            IAuthenticationSchemeProvider schemeProvider, 
            IOptionsMonitor<OpenIdConnectOptions> oidcOptions)
        {
            _httpClientFactory = httpClientFactory;
            _context = context.HttpContext;
            _schemeProvider = schemeProvider;
            _oidcOptions = oidcOptions;
            _tenantWebApiUrl = _context.User.FindFirst(c => c.Type.Contains("webapi_url")).Value;
        }

        public async Task<string> Get(string path, string fullpath = null, StringDictionary extraHeaders = null, StringDictionary returnHeaders = null, string acceptContentType = null)
        {
            return await Execute(httpVerb: HttpMethod.Get, path: path, fullpath: fullpath, returnHeaders: returnHeaders, contentType: acceptContentType, extraHeaders: extraHeaders);
        }

        public async Task<string> Patch(string path, HttpContent content, string fullpath = null, StringDictionary returnHeaders = null, StringDictionary extraHeaders = null)
        {
            return await Execute(httpVerb: new HttpMethod("PATCH"), path: path, content: content, fullpath: fullpath, returnHeaders: returnHeaders, extraHeaders: extraHeaders);
        }

        public async Task<string> Post(string path, HttpContent content, string fullpath = null, StringDictionary extraHeaders = null, StringDictionary returnHeaders = null)
        {
            return await Execute(httpVerb: HttpMethod.Post, path: path, content: content, extraHeaders: extraHeaders, returnHeaders: returnHeaders, fullpath: fullpath);
        }

        public async Task<string> Put(string path, HttpContent content, string fullpath = null, StringDictionary extraHeaders = null, StringDictionary returnHeaders = null)
        {
            return await Execute(httpVerb: HttpMethod.Put, path: path, content: content, fullpath: fullpath, extraHeaders: extraHeaders, returnHeaders: returnHeaders);
        }

        public async Task<string> Delete(string path, string fullpath = null)
        {
            return await Execute(httpVerb: HttpMethod.Delete, path: path, fullpath: fullpath);
        }

        private async Task<string> Execute(bool auth = true, string user = "", string password = "", HttpMethod httpVerb = null, string path = "", string fullpath = null, HttpContent content = null, string contentType = null, StringDictionary extraHeaders = null, StringDictionary returnHeaders = null, DateTime? lastModifiedSince = null, DateTime? unmodifiedSince = null, string language = null, bool chunked = false)
        {
            var accessToken = await GetAccessToken();

            var requestUri = GetUri(path, fullpath);
            
            SetHttpClient(auth, user: accessToken, password, contentType, lastModifiedSince, unmodifiedSince, language, requestUri, chunked);

            if (extraHeaders != null)
                foreach (string header in extraHeaders.Keys)
                {
                    if (header.ToLower() == "accept")
                        _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.TryAddWithoutValidation(header, extraHeaders[header]);
                }

            using (_client)
            {
                HttpResponseMessage msg = await GetResponse(httpVerb, content, requestUri);
                
                if (msg == null)
                {
                    return null;
                }
#if DEBUG
                Console.WriteLine("HTTP Return {0}", msg.StatusCode);
#endif

                msg.EnsureSuccessStatusCode();

                if (returnHeaders != null)
                {
                    returnHeaders.Clear();
                    returnHeaders["ContentType"] = msg.Content?.Headers?.ContentType?.ToString();
                    returnHeaders["Server"] = msg.Headers?.Server?.ToString();

                    returnHeaders["AllowOrigin"] = GetHeader(msg.Headers, "Access-Control-Allow-Origin");
                    returnHeaders["AllowMethods"] = GetHeader(msg.Headers, "Access-Control-Allow-Methods");
                    returnHeaders["PoweredBy"] = GetHeader(msg.Headers, "X-Powered-by");
                    returnHeaders["TimeZone"] = GetHeader(msg.Headers, "SO-TimeZone");
                }

                return await msg.Content.ReadAsStringAsync();
            }
        }

        private async Task<HttpResponseMessage> GetResponse(HttpMethod httpVerb, HttpContent content, Uri requestUri)
        {
            // apply defaults
            httpVerb = httpVerb ?? HttpMethod.Get;

            if (httpVerb == HttpMethod.Get)
                return await _client.GetAsync(requestUri);
            if (httpVerb == HttpMethod.Post)
                return await _client.PostAsync(requestUri, content);
            if (httpVerb == HttpMethod.Put)
                return await _client.PutAsync(requestUri, content);
            if (httpVerb == HttpMethod.Delete)
                return await _client.DeleteAsync(requestUri);
            if (httpVerb.Method.ToUpper() == "PATCH")
            {
                var req = new HttpRequestMessage(httpVerb, requestUri);
                req.Content = content;
                return await _client.SendAsync(req);
            }
            if (httpVerb == HttpMethod.Options)
            {
                var req = new HttpRequestMessage(httpVerb, requestUri);
                req.Content = content;
                return await _client.SendAsync(req);
            }
            throw new NotSupportedException("Requested something not understood.");
            // return null;
        }

        private void SetHttpClient(bool auth, string user, string password, string contentType, DateTime? lastModifiedSince, DateTime? unmodifiedSince, string language, Uri requestUri, bool chunked)
        {
            // apply defaults
            contentType = contentType ?? "application/json";

            _client = _httpClientFactory.CreateClient();

            _client.BaseAddress = requestUri;

            _client.Timeout = new TimeSpan(0, 1, 0);

            if (lastModifiedSince != null)
                _client.DefaultRequestHeaders.IfModifiedSince = lastModifiedSince;
            if (unmodifiedSince != null)
                _client.DefaultRequestHeaders.IfUnmodifiedSince = unmodifiedSince;
            if (language != null)
                _client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(language));
            if (chunked)
                _client.DefaultRequestHeaders.TransferEncodingChunked = true;
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
            if (auth && !string.IsNullOrEmpty(_defaultUserName))
            {
                var encoding = System.Text.Encoding.GetEncoding("iso-8859-1");
                string credentials = _defaultUserName + ":" + _defaultPassword;
                if (!string.IsNullOrEmpty(user))
                    credentials = user + ":";
                else
                    user = _defaultUserName;
                if (!string.IsNullOrEmpty(password))
                    credentials += password;
                if (user.StartsWith("7T:"))
                {
                    credentials = user;
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SOTicket", credentials);
                }
                else
                if (user.StartsWith("8A:"))
                {
                    credentials = user;
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials);
                }
                else
                {
                    credentials = Convert.ToBase64String(encoding.GetBytes(credentials), Base64FormattingOptions.None);
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
                }
            }
        }

        private Uri GetUri(string path, string fullpath)
        {
            if (fullpath != null)
            {
                if (fullpath.Contains("://"))
                    path = fullpath;
                else
                    path = _defaultUrl + fullpath;
            }
            else
            {
                path = path ?? "";
                if (path.Length > 0)
                    if (path.StartsWith("?"))
                        path = _tenantWebApiUrl + path;
                    else
                        path = CombineUrlParts(_tenantWebApiUrl, path);
                else
                    path = _tenantWebApiUrl;
            }

            return new Uri(path);
        }

        private string CombineUrlParts(string prefix, string suffix)
        {
            if (prefix.EndsWith("/"))
            {
                prefix = prefix.Substring(0, prefix.Length - 1);
            }

            if (suffix.StartsWith("/"))
            {
                suffix = suffix.Substring(1);
            }

            return prefix + "/" + suffix;
        }

        private string GetHeader(HttpResponseHeaders headers, string name)
        {
            string res = null;
            if (headers != null)
            {
                IEnumerable<string> values = null;
                if (headers.TryGetValues(name, out values))
                {
                    res = "";
                    foreach (var s in values)
                        res += (s ?? "") + " ";
                    res = res.Trim();
                }
            }
            return res;
        }

        private async Task<string> GetAccessToken()
        {
            string accessToken = string.Empty;

            var authService = _context.RequestServices.GetRequiredService<IAuthenticationService>();
            AuthenticateResult authenticateResult = await authService.AuthenticateAsync(_context, null);
            AuthenticationProperties properties = authenticateResult.Properties;
            var expiresAt = properties.GetTokenValue("expires_at");

            DateTime accessTokenExpiresAt = DateTime.Parse(expiresAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            
            if (accessTokenExpiresAt > DateTime.UtcNow)
            {
                accessToken = properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            }
            else
            {
                var refreshToken = properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
                accessToken = await RefreshAccessTokenAsync(refreshToken);
            }

            return accessToken;
        }

        private async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            var (options, configuration) = await GetOpenIdConnectSettingsAsync(OpenIdConnectDefaults.AuthenticationScheme);

            var parameters = new System.Collections.Generic.Dictionary<string, string> {
                { OpenIdConnectParameterNames.ClientId, options.ClientId },
                { OpenIdConnectParameterNames.ClientSecret, options.ClientSecret },
                { OpenIdConnectParameterNames.RefreshToken, refreshToken },
                { OpenIdConnectParameterNames.RedirectUri, options.CallbackPath},
                { OpenIdConnectParameterNames.GrantType, OpenIdConnectParameterNames.RefreshToken }
            };

            var encodedContent = new FormUrlEncodedContent(parameters);


            var tokenResponse = await options.Backchannel.PostAsync(
                configuration.TokenEndpoint, encodedContent, _context.RequestAborted);

            tokenResponse.EnsureSuccessStatusCode();

            var responseStream = await tokenResponse.Content.ReadAsStringAsync();
            
            //var payLoad = System.Text.Json.JsonDocument
            //props.UpdateTokenValue("access_token", payload.RootElement.GetString("access_token"));
            //props.UpdateTokenValue("refresh_token", payload.RootElement.GetString("refresh_token"));
            //if (payload.RootElement.TryGetProperty("expires_in", out var property) && property.TryGetInt32(out var seconds))
            //{
            //    var expiresAt = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(seconds);
            //    props.UpdateTokenValue("expires_at", expiresAt.ToString("o", CultureInfo.InvariantCulture));
            //}.Parse(responseStream);
            
            var json = JObject.Parse(responseStream);
            string accessToken = json[OpenIdConnectParameterNames.AccessToken].Value<string>();
            string idToken = json[OpenIdConnectParameterNames.IdToken].Value<string>();
            //string tokenType = json[OpenIdConnectParameterNames.TokenType].Value<string>(); //we know what it is...
            int expiresIn = json[OpenIdConnectParameterNames.ExpiresIn].Value<int>();

            // TY: Should this be UtcNow? I have my doubts... 
            var expiresAt = (DateTime.UtcNow + TimeSpan.FromSeconds(expiresIn)).ToString("o", CultureInfo.InvariantCulture);
            
            var authenticatedContext = await _context.AuthenticateAsync();
            var authProperties = authenticatedContext.Properties;
            authProperties.UpdateTokenValue(OpenIdConnectParameterNames.AccessToken, accessToken);
            authProperties.UpdateTokenValue(OpenIdConnectParameterNames.IdToken, idToken);
            authProperties.UpdateTokenValue(OpenIdConnectParameterNames.ExpiresIn, expiresIn.ToString());
            //authProperties.UpdateTokenValue(OpenIdConnectParameterNames.TokenType, tokenType);
            authProperties.UpdateTokenValue("expires_at", expiresAt);
            await _context.SignInAsync(_context.User, authProperties);
            return accessToken;

        }

        private async Task<(OpenIdConnectOptions options, OpenIdConnectConfiguration configuration)> GetOpenIdConnectSettingsAsync(string schemeName)
        {
            OpenIdConnectOptions options;

            if (string.IsNullOrWhiteSpace(schemeName))
            {
                var scheme = await _schemeProvider.GetDefaultChallengeSchemeAsync();

                if (scheme is null)
                {
                    throw new InvalidOperationException("No OpenID Connect authentication scheme configured for getting client configuration. Either set the scheme name explicitly or set the default challenge scheme");
                }

                options = _oidcOptions.Get(scheme.Name);
            }
            else
            {
                options = _oidcOptions.Get(schemeName);
            }

            OpenIdConnectConfiguration configuration;
            try
            {
                configuration = await options.ConfigurationManager.GetConfigurationAsync(_context.RequestAborted);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to load OpenID configuration for configured scheme: {e.Message}");
            }

            return (options, configuration);
        }
    }
}
