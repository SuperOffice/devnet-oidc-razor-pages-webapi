using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public class DeleteModel : PageModel
    {
        private readonly SuperOffice.DevNet.Asp.Net.RazorPages.Data.ContactDbContext _context;

        public DeleteModel(ContactDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await _context.Contacts.FirstOrDefaultAsync(m => m.ContactId == id);

            if (Contact == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Contact == null || !(Contact.ContactId > 0))
            {
                return NotFound();
            }
            
            await _context.Delete(Contact.ContactId);
            
            return RedirectToPage("./Index", new { message = $"Contact '{Contact.ContactId}' deleted!" });
        }
    }
}
