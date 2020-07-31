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
    public class IndexModel : PageModel
    {
        private readonly ProvisioningDbContext _context;
        private readonly IToastNotification _toastNotification;

        public IndexModel(ProvisioningDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        public bool ShowAll { get; set; } = false;

        public IList<WebPanelEntityModel> WebPanelList { get; set; }

        public async Task OnGetAsync(string message)
        {
            await LoadWebPanelsAsync();

            if (!string.IsNullOrEmpty(message))
            {
                _toastNotification.AddInfoToastMessage(message);
            }

        }

        private async Task LoadWebPanelsAsync()
        {
            var webPanels = await _context.GetAllWebPanels();
            WebPanelList = webPanels.OrderBy(w => w.Name).ToList();
        }
    }
}