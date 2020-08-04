using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuperOffice.DevNet.Asp.Net.RazorPages.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity
{
    public class LocalSignInManager : ISignInManager
    {
        private const string LoginProviderKey = "LoginProvider";
        private const string XsrfKey = "XsrfId";

        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUserManager userManager;

        public LocalSignInManager(IHttpContextAccessor contextAccessor, IUserManager userManager)
        {
            this.contextAccessor = contextAccessor;
            this.userManager = userManager;
        }

        public virtual async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string expectedXsrf = null)
        {
            var auth = await contextAccessor.HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            
            var items = auth?.Properties?.Items;
            if (auth?.Principal == null || items == null || !items.ContainsKey(LoginProviderKey))
            {
                return null;
            }

            if (expectedXsrf != null)
            {
                if (!items.ContainsKey(XsrfKey))
                {
                    return null;
                }
                var userId = items[XsrfKey] as string;
                if (userId != expectedXsrf)
                {
                    return null;
                }
            }

            // get the users unique id from the IdP
            var providerKey = auth.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var provider = items[LoginProviderKey] as string;
            if (providerKey == null || provider == null)
            {
                return null;
            }

            var providerDisplayName = (await contextAccessor.HttpContext.GetExternalProvidersAsync()).FirstOrDefault(p => p.Name == provider)?.DisplayName
                                      ?? provider;

            return new ExternalLoginInfo(auth.Principal, provider, providerKey, providerDisplayName)
            {
                AuthenticationTokens = auth.Properties.GetTokens(),
                AuthenticationProperties = auth.Properties
            };
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> SignInExternalUserAsync(string providerName, string identifier)
        {
            var user = await userManager.FindByLoginAsync(providerName, identifier);
            return await SignInExternalUserAsync(providerName, user);
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> SignInExternalUserAsync(string providerName, User user)
        {
            if (user == null)
                return Microsoft.AspNetCore.Identity.SignInResult.Failed;

            // Cleanup external cookie if necessary
            if (providerName != null)
            {
                await contextAccessor.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            }

            await SignInUserAsync(user, providerName);

            return Microsoft.AspNetCore.Identity.SignInResult.Success;
        }

        public async Task<bool> SignInUserAsync(string userName, string password)
        {
            var user = await userManager.Authenticate(userName, password);

            if(string.IsNullOrEmpty(user.UserName))
            {
                return false;
            }

            await SignInUserAsync(user);

            return true;

        }

        public async Task SignInUserAsync(User user, string providerScheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            var authInfo = await contextAccessor.HttpContext.AuthenticateAsync(providerScheme);

            List<Claim> claims;

            if (authInfo.Succeeded)
            {
                // arrived via external provider
                claims = new List<Claim>(authInfo.Principal.Claims);

                var props = new AuthenticationProperties();
                props.StoreTokens(authInfo.Properties.GetTokens());
                
                var identity = new ClaimsIdentity(claims, providerScheme);
                var principal = new ClaimsPrincipal(identity);

                await contextAccessor.HttpContext.SignInAsync(principal, props);
            }
            else
            {
                claims = new List<Claim> 
                { 
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                var identity = new ClaimsIdentity(claims, providerScheme);
                var principal = new ClaimsPrincipal(identity);

                await contextAccessor.HttpContext.SignInAsync(principal);
            }
        }
    }
}
