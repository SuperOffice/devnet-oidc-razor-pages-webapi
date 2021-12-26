using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Install.Webhooks
{
    public class EditModel : PageModel
    {
        private readonly ProvisioningDbContext _context;

        public EditModel(ProvisioningDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        // To protect from over-posting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _context.UpdateWebhook(WebhookModel);

            return RedirectToPage("./Index", new { message = $"{WebhookModel.Name} updated!" });
        }

        private bool WebhookModelExists(int id)
        {
            return _context.Webhooks.Any(e => e.WebhookId == id);
        }
    }
}
