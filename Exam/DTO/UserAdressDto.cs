using System.ComponentModel.DataAnnotations;

namespace Exam.DTO
{
    public class UserAddressDto
    {
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        public string FlatNumber { get; set; }
    }
}