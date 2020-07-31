using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NToastNotify;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Install.WebPanels
{
    public class CreateModel : PageModel
    {
        private readonly IToastNotification _toastNotification;

        private readonly ProvisioningDbContext _context;

        public CreateModel(ProvisioningDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public IList<ListItemModel> UserGroups { get; set; }

        [BindProperty]
        public List<SelectListItem> VisibleIn { get; set; }

        [BindProperty]
        public WebPanelEntityModel WebPanel { get; set; }

        public async Task<IActionResult> OnGet()
        {
            VisibleIn = VisibleInHelper.GetSelectListItems();
            UserGroups = await _context.GetWebPanelVisibleForUserGroups(0);
            WebPanel = await _context.CreateDefaultWebPanel();// Contact.CreateDefault(_client);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            WebPanel.WindowName = WebPanel.Name.ToLowerInvariant();
            WebPanel.OnSalesMarketingWeb = true;

            var webPanel = await _context.SaveAppWebPanel(WebPanel);
            await _context.SetVisibleForUserGroups(webPanel.WebPanelId, UserGroups);
            //_toastNotification.AddSuccessToastMessage($"New contact '{contact.Name}' saved!");
            return RedirectToPage("./Index", new { message = $"New web panel '{webPanel.Name}' created!" });
        }
    }
}
