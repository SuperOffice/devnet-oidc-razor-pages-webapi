using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Install.Webhooks
{
    public class DetailsModel : PageModel
    {
        private readonly ProvisioningDbContext _context;

        public DetailsModel(ProvisioningDbContext context)
        {
            _context = context;
        }

        public WebhookModel WebhookModel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WebhookModel = await _context.Webhooks.FirstOrDefaultAsync(m => m.WebhookId == id);

            if (WebhookModel == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
