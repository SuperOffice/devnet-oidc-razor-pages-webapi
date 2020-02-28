using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public class LoginModel : PageModel
    {
        public async Task OnGet()
        {
            await OnGetLogin();
        }

        public async Task OnGetLogin(string returnUrl = "/")
        {
            await HttpContext.ChallengeAsync("SuperOffice", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

    }
}