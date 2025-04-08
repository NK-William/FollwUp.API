using TaskStatus = FollwUp.API.Enums.TaskStatus;

namespace FollwUp.API.Model.Domain
{
    public class Task
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int ProgressToHundred { get; set; }
        public required string Organization { get; set; }
        public TaskStatus Status { get; set; }
        public string? Description { get; set; }
        public DateTime Eta { get; set; }
        public required string Color { get; set; }

        // The following properties have been added to cater for first version
        public required string ClientFirstName { get; set; }
        public required string ClientLastName { get; set; }
        public required string ClientEmail { get; set; }
        public required string ClientPhone { get; set; }
    }
}
