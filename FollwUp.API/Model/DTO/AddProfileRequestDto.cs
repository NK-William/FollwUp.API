namespace FollwUp.API.Model.DTO
{
    public class AddProfileRequestDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
