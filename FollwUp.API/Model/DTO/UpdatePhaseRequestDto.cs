using FollwUp.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class UpdatePhaseRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name text is too long")]
        public required string Name { get; set; }

        [Required]
        public int Number { get; set; }

        [MaxLength(1000, ErrorMessage = "Description text is too long")]
        public string? Description { get; set; }

        public string? Icon { get; set; }

        public TaskPhaseStatus Status { get; set; }

        [Required]
        public Guid TaskId { get; set; }

        public UpdatePhaseRequestDto()
        {
            TaskId = Guid.Empty;
        }
    }
}
