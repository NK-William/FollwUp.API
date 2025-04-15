using FollwUp.API.Model.Domain;

namespace FollwUp.API.Repositories.Interfaces
{
    public interface IProfileRepository
    {
        Task<Profile> CreateAsync(Profile profile);
        Task<Profile?> GetByEmailAsync(string email);
        Task<Profile?> UpdateAsync(Profile profile);
    }
}
