using FollwUp.API.Enums;

namespace FollwUp.API.Model.Domain
{
    public class Phase
    {
        public Guid id { get; set; }
        public required string Name { get; set; }
        public int Number { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public IconType IconType { get; set; }
        public TaskPhaseStatus Status { get; set; }
        public Guid TaskId { get; set; }

        // Navigation properties
        public required Task Task { get; set; }
    }
}
