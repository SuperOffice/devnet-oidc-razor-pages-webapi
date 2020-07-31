using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Install.WebPanels
{
    public class DeleteModel : PageModel
    {
        private readonly SuperOffice.DevNet.Asp.Net.RazorPages.Data.ProvisioningDbContext _context;

        public DeleteModel(ProvisioningDbContext context)
        {
            _context = context;
        }

        public bool Permanent { get; set; } = false;

        [BindProperty]
        public WebPanelEntityModel WebPanel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WebPanel = await _context.WebPanels.FirstOrDefaultAsync(m => m.WebPanelId == id);

            if (WebPanel == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool permanent)
        {
            if (WebPanel == null || !(WebPanel.WebPanelId > 0))
            {
                return NotFound();
            }
            
            await _context.DeleteWebPanel(WebPanel.WebPanelId, permanent);
            
            return RedirectToPage("./Index", new { message = $"Web panel '{WebPanel.Name}' deleted!" });
        }
    }
}
