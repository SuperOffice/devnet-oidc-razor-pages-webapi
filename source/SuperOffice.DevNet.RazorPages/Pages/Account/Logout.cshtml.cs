using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public class LogoutModel : PageModel
    {
        public async Task OnGet()
        {
            await OnGetLogout();
        }

        public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync();

            //await HttpContext.SignOutAsync("SuperOffice", new AuthenticationProperties
            //{
            //    // Indicate here where Auth0 should redirect the user after a logout.
            //    // Note that the resulting absolute Uri must be whitelisted in the
            //    // **Allowed Logout URLs** settings for the app.
            //    RedirectUri = "/"
            //});
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Index", new { message = "Logged out!" });
        }
    }
}