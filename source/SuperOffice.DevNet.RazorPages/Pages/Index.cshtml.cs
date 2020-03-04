using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NToastNotify;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages
{
    public class IndexModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IToastNotification _toastNotification;

        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor contextAccessor, IToastNotification toastNotification)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
            _toastNotification = toastNotification;
        }

        public IEnumerable<AuthenticationToken> Tokens { get; private set; }

        public async void OnGet()
        {
            var result = await _contextAccessor.HttpContext.AuthenticateAsync();

            if (result.Succeeded)
            {
                Tokens = result.Properties.GetTokens();
            }

            _toastNotification.AddInfoToastMessage("Welcome!");
        }
    }
}
