using System.ComponentModel.DataAnnotations;

namespace Exam
{
    public class UserAddress
    {
        [Key]
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string? FlatNumber { get; set; }
    }
}