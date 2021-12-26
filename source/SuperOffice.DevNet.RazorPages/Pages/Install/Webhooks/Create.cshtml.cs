using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Pages.Install.Webhooks
{
    public class CreateModel : PageModel
    {
        private readonly ProvisioningDbContext _context;
        
        [BindProperty]
        public WebhookModel Webhook { get; set; }

        public CreateModel(ProvisioningDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Webhook = await _context.CreateDefaultWebhook();
            return Page();
        }

        [BindProperty]
        public WebhookModel WebhookModel { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var webhook = await _context.SaveAppWebhook(WebhookModel);

            return RedirectToPage("./Index", new { message = $"New webhook '{webhook.Name}' created!" });
        }
    }
}
