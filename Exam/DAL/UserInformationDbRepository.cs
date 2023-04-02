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
        public void AddNewUserToList(int id, PersonalInformationDto UserDto)
        {

            var userFromDb = _context.UserAccounts.FirstOrDefault(i => i.Id == id);

            userFromDb.PersonalInformation = new PersonalInformation
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
                .Include(b => b.PersonalInformation)
                .ThenInclude(b => b.Address)
                .FirstOrDefault(i => i.Id == accId);

            if (userFromDb.PersonalInformation == null)
            {
                userFromDb.PersonalInformation = new PersonalInformation();
            }

            userFromDb.PersonalInformation.Name = name;
            userFromDb.PersonalInformation.Surname = surname;
            userFromDb.PersonalInformation.PersonalCode = personalCode;
            userFromDb.PersonalInformation.TelephoneNumber = phone;
            userFromDb.PersonalInformation.Email = email;

            if (userFromDb.PersonalInformation.Address == null)
            {
                userFromDb.PersonalInformation.Address = new UserAddress();
            }

            userFromDb.PersonalInformation.Address.City = city;
            userFromDb.PersonalInformation.Address.Street = street;
            userFromDb.PersonalInformation.Address.HouseNumber = houseNumber;
            userFromDb.PersonalInformation.Address.FlatNumber = string.IsNullOrEmpty(flatNumber) ? null : flatNumber;

            _context.SaveChanges();
        }
        public List<UserInformationDto> GetAllAccounts()
        {
            return _context.UserAccounts.Select(x => new UserInformationDto
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
            var accounts = _context.UserAccounts.Include(b => b.PersonalInformation).ThenInclude(b => b.UserAddress).ToList();
            List<UserDto> userDtos = new List<UserDto>();
            foreach (var acc in accounts)
            {
                if (acc.PersonalInformation != null)
                {
                    var userInfo = acc.PersonalInformation;
                    if (userInfo.UserAddress != null)
                    {
                        var address = userInfo.UserAddress;
                        var userDto = new UserDto
                        {
                            Username = acc.Username,
                            Role = acc.Role,
                            UserInformation = new PersonalInformationDto
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
                            UserInformation = new PersonalInformationDto
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
                .Include(b => b.PersonalInformation)
                .ThenInclude(b => b.Address)
                .FirstOrDefault(a => a.Id == userId);

            if (account == null)
            {
                return null;
            }
            var userInfo = account.PersonalInformation;
            var address = userInfo?.Address;
            var userDto = new UserDto
            {
                Username = account.Username,
                Role = account.Role
            };
            if (userInfo != null)
            {
                userDto.PersonalInformation = new PersonalInformationDto
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