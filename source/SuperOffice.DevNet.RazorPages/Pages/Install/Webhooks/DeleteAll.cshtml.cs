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
    public class DeleteAllModel : PageModel
    {
        private readonly ProvisioningDbContext _context;
        private readonly IToastNotification _toastNotification;

        public DeleteAllModel(ProvisioningDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public IList<WebhookModel> WebHooks { get; set; }

        public async Task OnGetAsync(string message)
        {
            await LoadWebhooksAsync();

            if (!string.IsNullOrEmpty(message))
            {
                _toastNotification.AddInfoToastMessage(message);
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _context.DeleteAllAppWebhooks();
            return RedirectToPage("./Index", new { message = $"All app-owned webhooks deleted!" });
        }

        private async Task LoadWebhooksAsync()
        {
            var webwebhooks = await _context.GetAllWebhooks();
            WebHooks = webwebhooks.OrderBy(w => w.Name).ToList();
        }
    }
}