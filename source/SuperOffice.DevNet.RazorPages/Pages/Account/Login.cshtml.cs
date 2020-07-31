using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperOffice.DevNet.Asp.Net.RazorPages.Extensions;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public class LoginModel : PageModel
    {
        private readonly IUserManager userManager;
        private readonly ISignInManager signInManager;
        private readonly IAuthenticationSchemeProvider schemeProvider;

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty]
        public LoginUserModel FormInput { get; set; }
        
        public string ReturnUrl { get; set; }

        
        public LoginModel(
            IUserManager userManager, 
            ISignInManager signInManager,
            IAuthenticationSchemeProvider schemeProvider)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.schemeProvider = schemeProvider;
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await HttpContext.GetExternalProvidersAsync()).ToList();
            
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await signInManager.SignInUserAsync(FormInput.Email, FormInput.Password);
                if (result)
                {
                    return LocalRedirect(Url.GetLocalUrl(returnUrl, $"Welcome {FormInput.Email}"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}