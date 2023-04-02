namespace Exam.BLL
{
    public interface IUserAccountService
    {
        UserAccount SignupNewAccount(string username, string password);
        (bool, UserAccount) Login(string username, string password);
    }
}