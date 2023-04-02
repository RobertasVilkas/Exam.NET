using Exam.DTO;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Exam.DAL
{
    public class UserInformationDbRepository : IUserInformationDbRepository
    {


        private readonly UserRegistrationDbContext _context;

        public UserInformationDbRepository(UserRegistrationDbContext context)
        {
            _context = context;
        }
        public void AddNewUserToList(int id, UserInformationDto UserDto)
        {

            var userFromDb = _context.UserAccounts.FirstOrDefault(i => i.Id == id);

            userFromDb.HumanInformation = new UserInformation
            {
                Name = UserDto.Name,
                Surname = UserDto.Surname,
                PersonalCode = UserDto.PersonalCode,
                TelephoneNumber = UserDto.TelephoneNumber,
                Email = UserDto.Email,
                Address = new UserAddress
                {
                    City = UserDto.Address.City,
                    Street = UserDto.Address.Street,
                    HouseNumber = UserDto.Address.HouseNumber,
                    FlatNumber = UserDto.Address.FlatNumber,
                },
            };
            _context.SaveChanges();
        }

        public void UpdateUserById(int accId, string name, string surname,
            string personalCode, string phone, string email,
            string city, string street, string houseNumber, string flatNumber)
        {
            var userFromDb = _context.UserAccounts
                .Include(b => b.HumanInformation)
                .ThenInclude(b => b.UserAddress)
                .FirstOrDefault(i => i.Id == accId);

            if (userFromDb.HumanInformation == null)
            {
                userFromDb.HumanInformation = new UserInformation();
            }

            userFromDb.UserInformation.Name = name;
            userFromDb.UserInformation.Surname = surname;
            userFromDb.UserInformation.PersonalCode = personalCode;
            userFromDb.UserInformation.TelephoneNumber = phone;
            userFromDb.UserInformation.Email = email;

            if (userFromDb.UserInformation.UserAddress == null)
            {
                userFromDb.UserInformation.UserAddress = new UserAddress();
            }

            userFromDb.UserInformation.UserAddress.City = city;
            userFromDb.UserInformation.UserAddress.Street = street;
            userFromDb.UserInformation.UserAddress.HouseNumber = houseNumber;
            userFromDb.UserInformation.UserAddress.FlatNumber = string.IsNullOrEmpty(flatNumber) ? null : flatNumber;

            _context.SaveChanges();
        }
        public List<UserInfoDto> GetAllAccounts()
        {
            return _context.UserAccounts.Select(x => new UserInfoDto
            {
                Username = x.Username,
                Role = x.Role,
            }).ToList();
        }

        public void DeleteUserById(int id)
        {
            _context.UserAccounts.Remove(new UserAccount { Id = id });
            _context.SaveChanges(true);
        }

        public IEnumerable<UserDto> GetAllUserInfo()
        {
            var accounts = _context.UserAccounts.Include(b => b.UserInformation).ThenInclude(b => b.Address).ToList();
            List<UserDto> accountDtos = new List<UserDto>();
            foreach (var acc in accounts)
            {
                if (acc.UserInformation != null)
                {
                    var userInfo = acc.userInformation;
                    if (userInfo.UserAddress != null)
                    {
                        var address = userInfo.UserAddress;
                        var accountDto = new UserDto
                        {
                            Username = acc.Username,
                            Role = acc.Role,
                            UserInformation = new UserInformationDto
                            {
                                Name = userInfo.Name,
                                Surname = userInfo.Surname,
                                PersonalCode = userInfo.PersonalCode,
                                TelephoneNumber = userInfo.TelephoneNumber,
                                Email = userInfo.Email,
                                Address = new UserAddressDto
                                {
                                    City = address.City,
                                    Street = address.Street,
                                    HouseNumber = address.HouseNumber,
                                    FlatNumber = address.FlatNumber
                                }
                            }
                        };
                        userDtos.Add(userDto);
                    }
                    else
                    {
                        var userDto = new UserDto
                        {
                            Username = acc.Username,
                            Role = acc.Role,
                            UserInformation = new UserInformationDto
                            {
                                Name = userInfo.Name,
                                Surname = userInfo.Surname,
                                PersonalCode = userInfo.PersonalCode,
                                TelephoneNumber = userInfo.TelephoneNumber,
                                Email = userInfo.Email,
                                Address = null
                            }
                        };
                        userDtos.Add(userDto);
                    }
                }
                else
                {
                    var userDto = new UserDto
                    {
                        Username = acc.Username,
                        Role = acc.Role,
                        UserInformation = null
                    };
                    userDtos.Add(userDto);
                }
            }
            return userDtos;
        }

        public UserDto GetUserInformationById(int userId)
        {
            var account = _context.UserAccounts
                .Include(b => b.UserInformation)
                .ThenInclude(b => b.UserAddress)
                .FirstOrDefault(a => a.Id == userId);

            if (account == null)
            {
                return null;
            }
            var userInfo = account.userInformation;
            var address = userInfo?.UserAddress;
            var userDto = new UserDto
            {
                Username = account.Username,
                Role = account.Role
            };
            if (userInfo != null)
            {
                userDto.UserInformation = new UserInformationDto
                {
                    Name = userInfo.Name,
                    Surname = userInfo.Surname,
                    PersonalCode = userInfo.PersonalCode,
                    TelephoneNumber = userInfo.TelephoneNumber,
                    Email = userInfo.Email
                };
            }
            if (address != null)
            {
                userDto.UserAddress = new UserAddressDto
                {
                    City = address.City,
                    Street = address.Street,
                    HouseNumber = address.HouseNumber,
                    FlatNumber = address.FlatNumber
                };
            }
            return userDto;
        }
    }
}