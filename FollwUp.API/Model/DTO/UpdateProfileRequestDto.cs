using System;
using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO;

public class UpdateProfileRequestDto
{       
        [Required]
        public Guid Id { get; set; } = Guid.Empty;

        [Required]
        [MaxLength(20, ErrorMessage = "FirstName text is too long")]
        public required string FirstName { get; set; }

        
        [Required]
        [MaxLength(20, ErrorMessage = "LastName text is too long")]
        public required string LastName { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }
}
