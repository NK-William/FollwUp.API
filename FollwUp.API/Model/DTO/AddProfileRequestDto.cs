using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class AddProfileRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "FirstName text is too long")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "LastName text is too long")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "EmailAddress text is too long")]
        public string EmailAddress { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
