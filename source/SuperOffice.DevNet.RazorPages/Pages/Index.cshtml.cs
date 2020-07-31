using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System.Collections.Generic;

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

        public async void OnGet(string message)
        {
            var result = await _contextAccessor.HttpContext.AuthenticateAsync();

            if (result.Succeeded)
            {
                Tokens = result.Properties.GetTokens();
            }
            
            if (!string.IsNullOrEmpty(message))
            {
                _toastNotification.AddInfoToastMessage(message);
                RouteData.Values.Remove("message");
            }
            else
            {
                _toastNotification.AddInfoToastMessage("Welcome!");
            }

        }
    }
}
