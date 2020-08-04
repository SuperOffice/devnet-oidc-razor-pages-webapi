using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperOffice.DevNet.Asp.Net.RazorPages.Extensions;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Account
{
    public class ExternalLoginModel : PageModel
    {
        private readonly ISignInManager signInManager;
        private readonly IUserManager userManager;

        public ExternalLoginModel(ISignInManager signInManager, IUserManager userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = new AuthenticationProperties { 
                RedirectUri = redirectUrl,
                Items =
                {
                    new KeyValuePair<string, string>("LoginProvider", provider)
                }
            };
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login");
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToPage("./Login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await signInManager.SignInExternalUserAsync(info.LoginProvider, info.ProviderKey);
            if (result.Succeeded)
            {
                return LocalRedirect(Url.GetLocalUrl(returnUrl));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                LoginProvider = info.LoginProvider;

                // Most IdPs use ClaimTypes.Email, but not all... 
                if (info.Principal.HasClaim(c => c.Type.Contains("email")))
                {
                    var claim = info.Principal.Claims.Where(c => c.Type.Contains("email")).FirstOrDefault();
                    Input = new InputModel
                    {
                        Email = claim.Value
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }

                var id = info.Principal.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c=>c.Value).FirstOrDefault();

                var user = await userManager.RegisterExternal(info.LoginProvider, id  ?? Input.Email, info.Principal.Identity.Name ?? Input.Email, Input.Email);
                var result = await signInManager.SignInExternalUserAsync(user.ProviderName, user);

                if (result.Succeeded)
                {
                    return LocalRedirect(Url.GetLocalUrl(returnUrl, "New external account created successfully!"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error creating external user.");
                }
            }

            ReturnUrl = returnUrl;
            return Page();
        }
    }
}