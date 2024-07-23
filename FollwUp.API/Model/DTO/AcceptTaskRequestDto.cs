using FollwUp.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class AcceptTaskRequestDto
    {
        [Required]
        public UpdateTaskRequestDto Task { get; set; }

        [Required]
        public Guid ProfileId { get; set; }

        [Required]
        public Guid InvitationId { get; set; }

        public RoleType RoleType { get; set; }

        public AcceptTaskRequestDto()
        {
            ProfileId = Guid.Empty;
            InvitationId = Guid.Empty;
        }
    }
}
