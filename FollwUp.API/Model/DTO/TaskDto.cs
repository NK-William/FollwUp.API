using FollwUp.API.Model.Domain;
using TaskStatus = FollwUp.API.Enums.TaskStatus;

namespace FollwUp.API.Model.DTO
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int ProgressToHundred { get; set; }
        public required string Organization { get; set; }
        public TaskStatus Status { get; set; }
        public string? Description { get; set; }
        public DateTime Eta { get; set; }
        public required string Color { get; set; }
        public required string ClientFirstName { get; set; }
        public  required string ClientLastName { get; set; }
        public required string ClientEmail { get; set; }
        public required string ClientPhone { get; set; }
        public required List<PhaseDto> Phases { get; set; }
        public required List<RoleDto> Roles { get; set; }
        public InvitationDto? Invitation { get; set; }
    }
}
