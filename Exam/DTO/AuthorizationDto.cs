using System.ComponentModel.DataAnnotations;

namespace Exam.DTO
{
    public class AuthorizationDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string UserName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Password { get; set; }
    }
}