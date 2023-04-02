using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeAcademyFinalExam
{
    public class UserAccount
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; }

        public int? HumanInformationId { get; set; }

        [ForeignKey("HumanInformationId")]
        public HumanInformation? HumanInformation { get; set; }
    }
}