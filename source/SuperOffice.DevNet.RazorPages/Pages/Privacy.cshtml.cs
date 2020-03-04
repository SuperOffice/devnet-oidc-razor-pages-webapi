using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NToastNotify;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly IToastNotification _toastNotification;

        public PrivacyModel(ILogger<PrivacyModel> logger, IToastNotification toastNotification)
        {
            _logger = logger;
            _toastNotification = toastNotification;
        }

        public void OnGet()
        {
            //_toastNotification.AddInfoToastMessage("Privacy is important");
        }
    }
}
