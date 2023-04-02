using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Exam
{
    public class UserRegistrationDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<Address> Address { get; set; }

        public DbSet<HumanInformation> HumanInformation { get; set; }

        public UserRegistrationDbContext(DbContextOptions<UserRegistrationDbContext> options) : base(options)
        {

        }
        public bool DoesUserExist(string username)
        {
            return Accounts.Any(a => a.Username == username);
        }
    }
}