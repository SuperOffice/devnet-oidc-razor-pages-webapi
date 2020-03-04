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
using NToastNotify;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models;

namespace SuperOffice.DevNet.Asp.Net.RazorPages
{
    public class IndexModel : PageModel
    {
        private readonly ContactDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IHttpContextAccessor _contextAccessor;

        public IndexModel(ContactDbContext context, IHttpContextAccessor contextAccessor, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
            _contextAccessor = contextAccessor;
        }

        public IList<Contact> ContactList { get;set; }

        public async Task LoadContactsAsync()
        {
            if(_context.Contacts != null && _context.Contacts.Count() > 0)
            {
                ContactList = _context.Contacts.ToList();
                return;
            }
            
            ContactList = await _context.GetAllContacts();
        }


        public async Task OnGetAsync(string message)
        {
            await LoadContactsAsync();
            if (!string.IsNullOrEmpty(message))
            {
                _toastNotification.AddInfoToastMessage(message);
            }

        }


    }
}
