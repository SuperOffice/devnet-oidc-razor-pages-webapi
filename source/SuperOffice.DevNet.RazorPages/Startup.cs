using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            //make sure the antiforgery cookies will be forwarded in iframe...
            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            // support older cookie settings, i.e. SameSiteMode.Undefined 
            services.ConfigureNonBreakingSameSiteCookies();

            services.AddHttpClient();
            services.AddDbContext<ContactDbContext>(options => options.UseInMemoryDatabase("superoffice"));
            services.AddDbContext<UserDbContext>(options => options.UseInMemoryDatabase("users"));
            services.AddDbContext<ProvisioningDbContext>(options => options.UseInMemoryDatabase("provisioner"));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddTransient<IUserManager, LocalUserManager>();
            services.AddTransient<ISignInManager, LocalSignInManager>();
            services.AddTransient<IHttpRestClient, SoHttpRestClient>();
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "SuperOffice"; // OpenIdConnectDefaults.AuthenticationScheme;
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
            .AddOpenIdConnect("SuperOffice", "SuperOffice", options =>
            {
                var suoEnv = Configuration.GetSection("SuperOffice");

                options.SignInScheme = IdentityConstants.ExternalScheme;

                options.Authority = $"https://{suoEnv["Environment"]}.superoffice.com/login";

                options.ClaimsIssuer = $"https://{suoEnv["Environment"]}.superoffice.com";

                options.ClientId = suoEnv["ClientId"];
                options.ClientSecret = suoEnv["ClientSecret"];

                options.RequireHttpsMetadata = true;
                options.ResponseType = "code";
                options.UsePkce = false;
                options.Scope.Add("openid");
                options.SaveTokens = true;
                options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/callback");
                options.ClaimActions.MapJsonKey(ClaimTypes.Email, "http://schemes.superoffice.net/identity/email");

                options.Events = new OpenIdConnectEvents
                {
                    // handle the logout redirection
                    OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        var logoutUri = $"https://{suoEnv["Environment"]}.superoffice.com/login/logout?scope=openid";

                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
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
                        //If the URL contains &uctx=, then include that in the URL to the SuperID /authorize endpoint, so that we get a better single-signon experience. 
                        //This is typically used when the webapplication is hosted as a webpanel inside SuperOffice.
                        var uctx = context.HttpContext.Request.Query["uctx"];
                        if (uctx.Count > 0)
                        {
                            // Replaces the /common/ section of the URL with the relevant SuperOffice tenant CustId
                            context.ProtocolMessage.IssuerAddress = context.ProtocolMessage.IssuerAddress.Replace("/login/common", "/login/" + uctx[0]);
                        }

                        return Task.CompletedTask; //Task.FromResult(0);
                    },
                };

            })
            .AddCookie(IdentityConstants.ExternalScheme);
            services.AddSession(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });

            // add notification services

            services.AddMvc()
                .AddRazorRuntimeCompilation()
                // AddMvc calls AddRazorPages, so all good here...
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Contacts");
                })
                .AddNToastNotifyToastr(null, new NToastNotify.NToastNotifyOption { DisableAjaxToasts = false }) ;

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.ForwardLimit = 4;
                //options.KnownProxies.Add(System.Net.IPAddress.Parse("127.0.10.1"));
                //options.ForwardedForHeaderName = "X-NToastNotify-Messages";
                //options.ForwardedHeaders.
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add this before any other middleware that might write cookies
            app.UseCookiePolicy();

            app.UseNToastNotify();
            // This will write cookies, so make sure it's after the cookie policy
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

            });


        }
    }
}
