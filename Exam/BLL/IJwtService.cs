namespace Exam.BLL
{
    public interface IJwtService
    {
        string GetJwtToken(string username, int accountId, string role);
    }
}
