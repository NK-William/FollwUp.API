namespace FollwUp.API.Model.DTO
{
    public class UpdateTaskRequestDto
    {
        public required string Name { get; set; }
        public int ProgressToHundred { get; set; }
        public required string Organization { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public string? Description { get; set; }
        public DateTime Eta { get; set; }
        public required string Color { get; set; }
    }
}
