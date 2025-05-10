using FollwUp.API.Enums;
using FollwUp.API.Model.Domain;
using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class AddPhaseRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name text is too long")]
        public string Name { get; set; }

        [Required]
        public int Number { get; set; }

        [MaxLength(1000, ErrorMessage = "Description text is too long")]
        public string? Description { get; set; }

        public Icon? Icon { get; set; }

        public TaskPhaseStatus Status { get; set; }

        [Required]
        public Guid TaskId { get; set; }

        public AddPhaseRequestDto()
        {
            TaskId = Guid.Empty;
        }
    }
}
