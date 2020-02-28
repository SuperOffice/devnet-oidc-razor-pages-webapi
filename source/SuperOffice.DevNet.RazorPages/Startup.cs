using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using System;
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
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IHttpRestClient, SoHttpRestClient>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "SuperOffice"; // OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            })
            .AddOpenIdConnect("SuperOffice", options =>
            {
                var suoEnv = Configuration.GetSection("SuperOffice");

                options.Authority = $"https://{suoEnv["Environment"]}.superoffice.com/login";

                options.ClaimsIssuer = $"https://sod.superoffice.com";

                options.ClientId = suoEnv["ClientId"];
                options.ClientSecret = suoEnv["ClientSecret"];

                options.RequireHttpsMetadata = true;
                options.ResponseType = "code";
                options.UsePkce = false;
                options.Scope.Add("openid");
                options.SaveTokens = true;
                options.CallbackPath = new Microsoft.AspNetCore.Http.PathString("/callback");

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
                    }
                };

            });
            services.AddSession(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });

            services.AddRazorPages()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Contacts");

                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
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
