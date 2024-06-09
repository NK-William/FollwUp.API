﻿using FollwUp.API.Enums;

namespace FollwUp.API.Model.DTO
{
    public class AddRoleRequestDto
    {

        public RoleType RoleType { get; set; }
        public Guid TaskId { get; set; }
        //public Guid ProfileId { get; set; } // TODO: Uncomment when profile is ready
        public AddRoleRequestDto()
        {
            TaskId = Guid.Empty;
            //ProfileId = Guid.Empty; // TODO: Uncomment when profile is ready
        }
    }
}
