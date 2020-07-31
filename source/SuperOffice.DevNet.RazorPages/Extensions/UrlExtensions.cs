using Microsoft.AspNetCore.Mvc;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Extensions
{
    public static class UrlExtensions
    {
        public static string GetLocalUrl(this IUrlHelper urlHelper, string localUrl, string notificationMessage = "")
        {
            if (!urlHelper.IsLocalUrl(localUrl))
            {
                if(!string.IsNullOrEmpty(notificationMessage))
                    return urlHelper.Page("/Index", new { message = notificationMessage });
                return urlHelper.Page("/Index");
            }

            return localUrl;
        }
    }
}
