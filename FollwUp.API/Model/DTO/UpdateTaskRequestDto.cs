﻿using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class UpdateTaskRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name text is too long")]
        public string Name { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "ProgressToHundred has to range between 0 and 100")]
        public int ProgressToHundred { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Organization text is too long")]
        public string Organization { get; set; }

        public Enums.TaskStatus Status { get; set; }


        [MaxLength(1000, ErrorMessage = "Description text is too long")]
        public string? Description { get; set; }

        [Required]
        public DateTime Eta { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Client first name text is too long")]
        public required string ClientFirstName { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Client last name text is too long")]
        public  required string ClientLastName { get; set; }

        public required string ClientEmail { get; set; }
        
        public required string ClientPhone { get; set; }
    }
}
