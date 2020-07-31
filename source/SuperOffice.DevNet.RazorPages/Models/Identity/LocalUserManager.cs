using SuperOffice.DevNet.Asp.Net.RazorPages.Data;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity
{
    public class LocalUserManager : IUserManager
    {
        private readonly UserDbContext userContext;

        public LocalUserManager(UserDbContext userDbContext)
        {
            this.userContext = userDbContext;
        }

        public Task<User> Authenticate(string email, string password)
        {
            var user = userContext.Users.Where(u => u.UserName.Equals(
                email, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                
            if (user != null)
            {
                var hashedPassword = HashString(password);
                if (user.PasswordHash == hashedPassword)
                {
                    return Task.FromResult(user);
                }
            }

            return Task.FromResult(new User());
        }

        public Task<User> FindByLoginAsync(string provider, string userName)
        {
            return userContext.FindUser(userName, provider);
        }

        public async Task<User> Register(string email, string password)
        {
            var user = new User()
            { 
                UserName = email,
                Email = email,
                PasswordHash = HashString(password)
            };

            return await userContext.AddUser(user);

        }

        public async Task<User> RegisterExternal(string provider, string email)
        {
            var user = new User()
            {
                UserName = email,
                Email = email,
                ProviderName = provider
            };

            return await userContext.AddUser(user);
        }

        public async Task<bool> UserExists(string email)
        {
            var user = userContext.Users.Where(u => u.UserName == email).FirstOrDefault();
            return await Task.FromResult(user != null);
        }

        private string HashString(string str)
        {
            var message = Encoding.Unicode.GetBytes(str);
            var hash = new SHA256Managed();

            var hashValue = hash.ComputeHash(message);
            return Encoding.Unicode.GetString(hashValue);
        }
    }
}
