using System.ComponentModel.DataAnnotations;
using TaskStatus = FollwUp.API.Enums.TaskStatus;

namespace FollwUp.API.Model.DTO
{
    public class AddTaskRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name text is too long")]
        public string Name { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "ProgressToHundred has to range between 0 and 100")]
        public int ProgressToHundred { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Organization text is too long")]
        public required string Organization { get; set; }

        public TaskStatus Status { get; set; }

        [MaxLength(1000, ErrorMessage = "Description text is too long")]
        public string? Description { get; set; }

        [Required]
        public DateTime Eta { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public Guid ProfileId { get; set; }

        [Required]
        public List<AddPhaseRequestDto> Phases { get; set; }

        [Required]
        public AddInvitationRequestDto Invitation { get; set; }
    }
}
