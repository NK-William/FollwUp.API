using FollwUp.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class AddInvitationRequestDto
    {
        [Required]
        public string PhoneNumber { get; set; }

        public RoleType RoleType { get; set; }

        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public UpdateTaskRequestDto? Task { get; set; }
    }
}
