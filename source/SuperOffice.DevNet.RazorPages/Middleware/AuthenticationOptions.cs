using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Middleware
{
    public class MiddlewareAuthenticationOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }
        public string TokenEndpoint { get; set; }
        public RemoteAuthenticationOptions Options { get; set; }
    }
}
