using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Exam
{
    public class UserRegistrationDbContext : DbContext
    {
        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<UserAddress> Address { get; set; }

        public DbSet<PersonalInformation> PersonalInformation { get; set; }

        public UserRegistrationDbContext(DbContextOptions<UserRegistrationDbContext> options) : base(options)
        {

        }
        public bool DoesUserExist(string username)
        {
            return UserAccounts.Any(a => a.Username == username);
        }
    }
}