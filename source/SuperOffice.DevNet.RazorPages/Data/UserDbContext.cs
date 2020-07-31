using Microsoft.EntityFrameworkCore;
using SuperOffice.DevNet.Asp.Net.RazorPages.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperOffice.DevNet.Asp.Net.RazorPages.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users{ get; set; }

        public async Task<User> AddUser(User user)
        {
            var efUser = Users.Add(user);
            await SaveChangesAsync();
            return efUser.Entity;
        }

        public async Task<User> FindUser(string userName, string provider)
        {
            var efUser = await Users.FirstOrDefaultAsync(u=>u.ProviderName == provider 
                && u.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
            return efUser;
        }
    }
}
