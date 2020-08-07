using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Make sure the antiforgery cookies will be forwarded in iframe...
            
            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // Support older cookie settings, i.e. SameSiteMode.Undefined 

            services.ConfigureNonBreakingSameSiteCookies();

            // Add support for HttpClient, for the REST client
            
            services.AddHttpClient();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IHttpRestClient, SoHttpRestClient>();

            // Add inmemory database contexts (for caching purposes).

            services.AddDbContext<ContactDbContext>(options => options.UseInMemoryDatabase("superoffice"));
            services.AddDbContext<UserDbContext>(options => options.UseInMemoryDatabase("users"));
            services.AddDbContext<ProvisioningDbContext>(options => options.UseInMemoryDatabase("provisioner"));

            // Use dependency injection for sign-in and user management.

            services.AddTransient<IUserManager, LocalUserManager>();
            services.AddTransient<ISignInManager, LocalSignInManager>();
            
            // Add authentication support.

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";

                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            })
            .AddTwitter(options =>
            {
                Configuration.Bind("Twitter", options);
                options.SignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddOpenIdConnect("SuperOffice", "SuperOffice", options =>
            {
                var suoEnv = Configuration.GetSection("SuperOffice");

                options.SignInScheme = IdentityConstants.ExternalScheme;
                
                options.ClientId = suoEnv["ClientId"]; 
                options.ClientSecret = suoEnv["ClientSecret"];
                options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/callback");
                options.Authority = $"https://{suoEnv["Environment"]}.superoffice.com/login";
                options.ClaimsIssuer = $"https://{suoEnv["Environment"]}.superoffice.com";
                options.Scope.Add("openid");
                options.SaveTokens = true;
                options.RequireHttpsMetadata = true;
                options.ResponseType         = "code";

                // This aligns the life of the cookie with the life of the token.
                // Note this is not the actual expiration of the cookie as seen by the browser.
                // It is an internal value stored in "expires_at".
                
                options.UseTokenLifetime     = true;

                // Only use Proof Key for Code Exchange (PKCE) for apps 
                // registered as Native or Mobile. 
                // When true, ClientId must be emtpy string ("";).

                options.UsePkce = false;

                // Use ClaimActions to standardize SuperOffice claims.

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "http://schemes.superoffice.net/identity/upn");
                options.ClaimActions.MapJsonKey(ClaimTypes.Email, "http://schemes.superoffice.net/identity/email");

                options.Events = new OpenIdConnectEvents
                {
                    // Handle the logout redirection.

                    OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        var logoutUri = $"https://{suoEnv["Environment"]}.superoffice.com/login/logout?scope=openid";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // Transform to absolute.

                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }
                            logoutUri += $"&redirect_uri={ Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    },
                    OnRedirectToIdentityProvider = context =>
                    {
                        // when hosted in a webpanel inside SuperOffice...
                        // if the URL contains uctx, include it for single-signon experience. 
                        
                        var uctx = context.HttpContext.Request.Query["uctx"];
                        if (uctx.Count > 0)
                        {
                            // Replaces login/common/ with login/Cust12345 

                            context.ProtocolMessage.IssuerAddress = 
                                context.ProtocolMessage.IssuerAddress
                                    .Replace("/login/common", "/login/" + uctx[0]);
                        }

                        return Task.CompletedTask;
                    }
                };

            })

            // Use as temporary cookie with OAuth/OIDC providers.

            .AddCookie(IdentityConstants.ExternalScheme);

            services.AddSession(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });


            services.AddMvc()
                .AddRazorRuntimeCompilation()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Contacts");
                    options.Conventions.AuthorizeFolder("/Install");
                })
            
                // Add notification services.
            
                .AddNToastNotifyToastr(null, new NToastNotify.NToastNotifyOption { DisableAjaxToasts = false }) ;

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.ForwardLimit = 4;
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Add middleware to help obtain new AccessToken when needed

            app.UseRefreshTokenMiddleware();

            app.UseRouting();

            app.UseCookiePolicy();

            app.UseNToastNotify();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

            });
        }
    }
}
