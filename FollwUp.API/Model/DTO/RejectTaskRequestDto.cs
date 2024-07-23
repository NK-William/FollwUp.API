using FollwUp.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class RejectTaskRequestDto
    {
        [Required]
        public UpdateTaskRequestDto Task { get; set; }

        [Required]
        public Guid InvitationId { get; set; }

        public RejectTaskRequestDto() => InvitationId = Guid.Empty;
    }
}
