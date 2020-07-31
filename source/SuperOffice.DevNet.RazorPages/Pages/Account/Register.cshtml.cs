using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperOffice.DevNet.Asp.Net.RazorPages.Extensions;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserManager userManager;
        private readonly ISignInManager signInManager;
        private readonly IAuthenticationSchemeProvider schemeProvider;

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [BindProperty]
        public RegisterUserModel FormInput { get; set; }

        public string ReturnUrl { get; set; }

        public RegisterModel(
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
            ReturnUrl = returnUrl;
            ExternalLogins = (await HttpContext.GetExternalProvidersAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? "~/";
            if (ModelState.IsValid)
            {
                if(!await userManager.UserExists(FormInput.Email))
                {
                    var userResult = await userManager.Register(FormInput.Email, FormInput.Password);
                    await signInManager.SignInUserAsync(userResult);

                    return RedirectToPage("/Index", new { message = "Registration successful!" });
                }
                else
                {
                    ModelState.AddModelError("Already registered", "Username already registed");
                }

            }

            // When error, redisplay form
            return Page();
        }
    }
}