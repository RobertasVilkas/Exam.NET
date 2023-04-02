using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.DTO
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; }

        public string Role { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public PersonalInformationDto UserInformation { get; internal set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public UserAddressDto UserAddress { get; internal set; }

        public static UserDto FromAccount(UserAccount account)
        {
            if (account == null)
            {
                return null;
            }

            var userDto = new UserDto
            {
                Username = account.Username,
                Role = account.Role
            };

            if (account.UserInformation != null)
            {
                userDto.UserInformation = new UserInformationDto
                {
                    Name = account.UserInformation.Name,
                    Surname = account.UserInformation.Surname,
                    PersonalCode = account.UserInformation.PersonalCode,
                    TelephoneNumber = account.UserInformation.TelephoneNumber,
                    Email = account.UserInformation.Email
                };

                if (account.PersonalInformation.UserAddress != null)
                {
                    userDto.UserAddress = new UserAddressDto
                    {
                        City = account.UserInformation.UserAddress.City,
                        Street = account.UserInformation.UserAddress.Street,
                        HouseNumber = account.UserInformation.UserAddress.HouseNumber,
                        FlatNumber = account.UserInformation.UserAddress.FlatNumber
                    };
                }
            }
            return userDto;
        }

    }
}