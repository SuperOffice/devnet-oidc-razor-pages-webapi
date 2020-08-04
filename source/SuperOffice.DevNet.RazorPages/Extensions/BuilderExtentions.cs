using SuperOffice.DevNet.Asp.Net.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseRefreshTokenMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RefreshTokenMiddleware>();
        }
    }
}
