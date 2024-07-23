using System.ComponentModel.DataAnnotations;

namespace FollwUp.API.Model.DTO
{
    public class DisconnectTaskRequestDto
    {
        [Required]
        public UpdateTaskRequestDto Task { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        public DisconnectTaskRequestDto()
        {
            RoleId = Guid.Empty;
        }
    }
}
