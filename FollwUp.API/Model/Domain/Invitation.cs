using FollwUp.API.Enums;

namespace FollwUp.API.Model.Domain
{
    public class Invitation
    {
        public Guid Id { get; set; }
        public required string PhoneNumber { get; set; }
        public required RoleType RoleType { get; set; }

        public Guid TaskId { get; set; }

        // Navigation properties
        public required Task Task { get; set; }
    }
}
