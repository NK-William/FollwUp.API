using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = "EmailAddress text is too long")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "FirstName text is too long")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "LastName text is too long")]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
