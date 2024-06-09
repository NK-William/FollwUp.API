using TaskStatus = FollwUp.API.Enums.TaskStatus;

namespace FollwUp.API.Model.DTO
{
    public class AddTaskRequestDto
    {
        public required string Name { get; set; }
        public int ProgressToHundred { get; set; }
        public required string Organization { get; set; }
        public TaskStatus Status { get; set; }
        public string? Description { get; set; }
        public DateTime Eta { get; set; }
        public required string Color { get; set; }
        public Guid ProfileId { get; set; }

        public required List<AddPhaseRequestDto> Phases { get; set; }
        public required AddInvitationRequestDto Invitation { get; set; }
    }
}
