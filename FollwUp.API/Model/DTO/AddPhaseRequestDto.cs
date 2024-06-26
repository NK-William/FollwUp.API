﻿using FollwUp.API.Enums;

namespace FollwUp.API.Model.DTO
{
    public class AddPhaseRequestDto
    {
        public required string Name { get; set; }
        public int Number { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public TaskPhaseStatus Status { get; set; }
        public Guid TaskId { get; set; }

        public AddPhaseRequestDto()
        {
            TaskId = Guid.Empty;
        }
    }
}
