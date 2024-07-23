using FollwUp.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class AddRoleRequestDto
    {
        public RoleType RoleType { get; set; }

        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public Guid ProfileId { get; set; }

        public AddRoleRequestDto()
        {
            TaskId = Guid.Empty;
            ProfileId = Guid.Empty;
        }
    }
}
