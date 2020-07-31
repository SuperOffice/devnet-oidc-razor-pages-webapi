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
    public class DetailsModel : PageModel
    {
        private readonly ProvisioningDbContext _context;

        public DetailsModel(ProvisioningDbContext context)
        {
            _context = context;
        }

        public WebPanelEntityModel WebPanel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WebPanel = await _context.WebPanels.FirstOrDefaultAsync(m => m.WebPanelId== id);

            if (WebPanel == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
