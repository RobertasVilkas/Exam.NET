using Exam.DTO;

namespace Exam.DAL
{
    public interface IUserInformationDbRepository
    {
        void AddNewUserToList(int id, UserInformationDto UserDto);
        void UpdateUserById(int id, string name, string surname,
            string personalCode, string phone, string email,
            string city, string street, string houseNumber, string flatNumber);
        public List<UserInformationDto> GetAllAccounts();
        public void DeleteUserById(int id);

        public IEnumerable<UserDto> GetAllUserInfo();

        public UserDto GetUserInformationById(int userId);

    }
}