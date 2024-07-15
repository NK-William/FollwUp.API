using FollwUp.API.Enums;

namespace FollwUp.API.Model.DTO
{
    public class AcceptTaskRequestDto
    {
        public required UpdateTaskRequestDto UpdateTaskRequestDto { get; set; }
        public Guid ProfileId { get; set; }
        public Guid InvitationId { get; set; }
        public RoleType RoleType { get; set; }

        public AcceptTaskRequestDto()
        {
            ProfileId = Guid.Empty;
            InvitationId = Guid.Empty;
        }
    }
}
