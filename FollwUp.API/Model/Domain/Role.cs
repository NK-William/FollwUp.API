using FollwUp.API.Enums;

namespace FollwUp.API.Model.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public RoleType RoleType { get; set; }

        public Guid TaskId { get; set; }
        public Guid ProfileId { get; set; }

        // Navigation properties
        public required Task Task { get; set; }
        public required Profile Profile { get; set; }
    }
}
