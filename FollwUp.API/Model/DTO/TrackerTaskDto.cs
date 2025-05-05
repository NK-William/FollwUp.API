using System;
using TaskStatus = FollwUp.API.Enums.TaskStatus;

namespace FollwUp.API.Model.DTO;

public class TrackerTaskDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int ProgressToHundred { get; set; }
    public required string Organization { get; set; }
    public TaskStatus Status { get; set; }
    public string? Description { get; set; }
    public DateTime Eta { get; set; }
    public required List<PhaseDto> Phases { get; set; }
}
