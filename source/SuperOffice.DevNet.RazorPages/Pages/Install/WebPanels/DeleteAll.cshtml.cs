using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Install.WebPanels
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
        public IList<WebPanelEntityModel> WebPanelList { get; set; }

        public async Task OnGetAsync(string message)
        {
            await LoadWebPanelsAsync();

            if (!string.IsNullOrEmpty(message))
            {
                _toastNotification.AddInfoToastMessage(message);
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _context.DeleteAllAppWebPanels();
            return RedirectToPage("./Index", new { message = $"All app-owned web panels deleted!" });
        }

        private async Task LoadWebPanelsAsync()
        {
            var webPanels = await _context.GetAllWebPanels();
            WebPanelList = webPanels.Where(p=>p.IsAppWebPanel).OrderBy(w => w.Name).ToList();
        }
    }
}