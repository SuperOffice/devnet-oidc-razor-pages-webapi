using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Install.Webhooks
{
    public class IndexModel : PageModel
    {
        private readonly ProvisioningDbContext _context;
        private readonly IToastNotification _toastNotification;

        public IndexModel(ProvisioningDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public IList<WebhookModel> WebhookList { get; set; }

        public async Task OnGetAsync(string message)
        {
            await LoadWebhooksAsync();

            if (!string.IsNullOrEmpty(message))
            {
                _toastNotification.AddInfoToastMessage(message);
            }

        }

        private async Task LoadWebhooksAsync()
        {
            var webhooks = await _context.GetAllWebhooks();
            WebhookList = webhooks.OrderBy(w => w.Name).ToList();
        }
    }
}