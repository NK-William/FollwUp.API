namespace FollwUp.API.Model.DTO
{
    public class AddInvitationRequestDto
    {
        public required string PhoneNumber { get; set; }
        public Guid TaskId { get; set; }
    }
}
