using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity
{
    public interface ISignInManager
    {
        Task<bool> SignInUserAsync(string userName, string password);
        Task SignInUserAsync(User user, string providerScheme = CookieAuthenticationDefaults.AuthenticationScheme);
        Task<SignInResult> SignInExternalUserAsync(string loginProvider, string userName);
        Task<SignInResult> SignInExternalUserAsync(string loginProvider, User user);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync(string xsrfToken = null);
    }
}
