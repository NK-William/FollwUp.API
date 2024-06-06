using FollwUp.API.Model.Domain;

namespace FollwUp.API.Repositories.Interfaces
{
    public interface IInvitationRepository
    {
        Task<Invitation> CreateAsync(Invitation invitation);
    }
}
