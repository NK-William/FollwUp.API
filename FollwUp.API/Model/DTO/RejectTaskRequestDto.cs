using FollwUp.API.Enums;

namespace FollwUp.API.Model.DTO
{
    public class RejectTaskRequestDto
    {
        public required UpdateTaskRequestDto Task { get; set; }
        public Guid InvitationId { get; set; }

        public RejectTaskRequestDto() => InvitationId = Guid.Empty;
    }
}
