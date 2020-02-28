using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public class IndexModel : PageModel
    {
        private readonly ContactDbContext _context;

        public IndexModel(ContactDbContext context)
        {
            _context = context;
        }

        public IList<Contact> ContactList { get;set; }

        public async Task OnGetAsync()
        {
            if(_context.Contacts != null && _context.Contacts.Count() > 0)
            {
                ContactList = _context.Contacts.ToList();
                return;
            }
            
            ContactList = await _context.GetAllContacts();
        }
    }
}
