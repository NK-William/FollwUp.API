using FollwUp.API.Enums;

namespace FollwUp.API.Model.DTO
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public RoleType RoleType { get; set; }

        public Guid TaskId { get; set; }
        public Guid ProfileId { get; set; }
    }
}
