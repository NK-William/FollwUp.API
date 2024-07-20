using FollwUp.API.Model.Domain;

namespace FollwUp.API.Repositories.Interfaces
{
    public interface IInvitationRepository
    {
        Task<Invitation> CreateAsync(Invitation invitation);
        Task<Invitation?> GetByTaskIdAsync(Guid taskId);
        Task<List<Invitation>> GetAllByPhoneNumberAsync(string phoneNumber);
        Task<List<Invitation>> GetAllByTaskAsync(Guid taskId);
        Task<Invitation?> DeleteAsync(Guid id);
    }
}
