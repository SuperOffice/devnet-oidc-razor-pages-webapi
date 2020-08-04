using Microsoft.AspNetCore.Identity;
using System;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity
{
    public class User : IdentityUser
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string ProviderName { get; set; }
        public string UniqueIdentifier { get; set; }
    }
}
