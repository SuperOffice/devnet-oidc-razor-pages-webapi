    using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Install.WebPanels
{
    public class EditModel : PageModel
    {
        private readonly ProvisioningDbContext _context;

        public EditModel(ProvisioningDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WebPanelEntityModel WebPanel { get; set; }
        [BindProperty]
        public IList<ListItemModel> UserGroups { get; set; }
        [BindProperty] 
        public List<SelectListItem> VisibleIn { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            VisibleIn = VisibleInHelper.GetSelectListItems();

            UserGroups = await _context.GetWebPanelVisibleForUserGroups(id.Value);

            WebPanel = await _context.WebPanels.FirstOrDefaultAsync(m => m.WebPanelId == id);

            if (WebPanel == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                VisibleIn = VisibleInHelper.GetSelectListItems();
                return Page();
            }

            await _context.UpdateWebPanel(WebPanel);
            await _context.SetVisibleForUserGroups(WebPanel.WebPanelId, UserGroups);

            return RedirectToPage("./Index", new { message = $"{WebPanel.Name} updated!"});
        }
    }
}
