namespace Exam.DAL
{
    public class JwtRepository : IJwtRepository
    {
        readonly UserRegistrationDbContext _context;

        public JwtRepository(UserRegistrationDbContext context)
        {
            _context = context;
        }

        public UserAccount GetAccount(string username)
        {
            return _context.UserAccounts.FirstOrDefault(A => A.Username == username);
        }

        public void SaveAccount(UserAccount account)
        {
            _context.UserAccounts.Add(account);
            _context.SaveChanges();
        }
    }
}
