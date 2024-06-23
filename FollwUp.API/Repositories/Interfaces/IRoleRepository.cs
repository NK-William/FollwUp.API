using FollwUp.API.Model.Domain;

namespace FollwUp.API.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> CreateAsync(Role role);

        Task<List<Role>> GetAllByProfileIdAsync(Guid id);
        Task<List<Role>> GetAllByTaskIdAsync(Guid id);

    }
}
