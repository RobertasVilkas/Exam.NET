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
        public PersonalInformationDto PersonalInformation { get; internal set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public UserAddressDto Address { get; internal set; }

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

            if (account.PersonalInformation != null)
            {
                userDto.PersonalInformation = new PersonalInformationDto
                {
                    Name = account.PersonalInformation.Name,
                    Surname = account.PersonalInformation.Surname,
                    PersonalCode = account.PersonalInformation.PersonalCode,
                    TelephoneNumber = account.PersonalInformation.TelephoneNumber,
                    Email = account.PersonalInformation.Email
                };

                if (account.PersonalInformation.Address != null)
                {
                    userDto.Address = new UserAddressDto
                    {
                        City = account.PersonalInformation.Address.City,
                        Street = account.PersonalInformation.Address.Street,
                        HouseNumber = account.PersonalInformation.Address.HouseNumber,
                        FlatNumber = account.PersonalInformation.Address.FlatNumber
                    };
                }
            }
            return userDto;
        }

    }
}