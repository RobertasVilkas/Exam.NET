using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam
{
    public class UserAccount
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; }

        public int? PersonalInformationId { get; set; }

        [ForeignKey("PersonalInformationId")]
        public PersonalInformation? PersonalInformation { get; set; }
    }
}