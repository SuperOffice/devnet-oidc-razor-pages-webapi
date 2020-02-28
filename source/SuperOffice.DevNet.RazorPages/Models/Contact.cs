using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models
{
    public class Contact
    {
        public Contact() {}

        public Contact(Contact contact)
        {
            Name = contact.Name;
            Department = contact.Department;
        }

        public int ContactId { get; set; }

        public string Name { get; set; }

        public string Department { get; set; }

    }
}
