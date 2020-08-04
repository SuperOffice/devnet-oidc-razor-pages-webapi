using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity
{
    public interface IUserManager
    {
        Task<bool> UserExists(string email);
        Task<User> Authenticate(string email, string password);
        Task<User> Register(string email, string password);
        Task<User> RegisterExternal(string provider, string identifier, string username, string email);
        Task<User> FindByLoginAsync(string providerName, string userName);
    }
}