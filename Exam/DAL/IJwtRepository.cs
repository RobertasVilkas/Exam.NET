using Microsoft.EntityFrameworkCore;

namespace Exam.DAL
{
    public interface IJwtRepository
    {
        void SaveAccount(UserAccount account);
        UserAccount GetAccount(string username);

    }
}