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
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public class CreateModel : PageModel
    {
        private readonly ContactDbContext _context;

        public CreateModel(ContactDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            // get default contact from SuperOffice with default values 
            // (ok... not so useful in this case, but maybe later)

            Contact = await _context.CreateDefault();// Contact.CreateDefault(_client);
            
            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var contact = await _context.Save(Contact);
           
            return RedirectToPage("./Index");
        }
    }
}
