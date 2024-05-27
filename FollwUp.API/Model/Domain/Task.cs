using FollwUp.API.Enums;
using System.Drawing;
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
    }
}
