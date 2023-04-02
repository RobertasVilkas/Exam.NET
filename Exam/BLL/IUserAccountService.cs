namespace Exam.BLL
{
    public interface IAccountService
    {
        UserAccount SignupNewAccount(string username, string password);
        (bool, UserAccount) Login(string username, string password);
    }
}