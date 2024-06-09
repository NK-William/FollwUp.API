using FollwUp.API.Enums;

namespace FollwUp.API.Model.DTO
{
    public class AddRoleRequestDto
    {

        public RoleType RoleType { get; set; }
        public Guid TaskId { get; set; }
        public Guid ProfileId { get; set; }
        public AddRoleRequestDto()
        {
            TaskId = Guid.Empty;
            ProfileId = Guid.Empty;
        }
    }
}
