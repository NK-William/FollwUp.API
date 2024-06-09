namespace FollwUp.API.Model.DTO
{
    public class InvitationDto
    {
        public Guid Id { get; set; }
        public required string PhoneNumber { get; set; }
        public Guid TaskId { get; set; }
    }
}
