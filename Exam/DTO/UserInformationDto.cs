using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.DTO
{
    public class UserInformationDto
    {
        [Required]
        public string Username { get; set; }

        public string Role { get; set; }
    }
}