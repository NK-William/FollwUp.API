namespace FollwUp.API.Model.DTO
{
    public class DisconnectTaskRequestDto
    {
        public required UpdateTaskRequestDto Task { get; set; }
        public Guid RoleId { get; set; }

        public DisconnectTaskRequestDto()
        {
            RoleId = Guid.Empty;
        }
    }
}
