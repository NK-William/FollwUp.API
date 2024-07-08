using FollwUp.API.Model.Domain;

namespace FollwUp.API.Repositories.Interfaces
{
    public interface IPhaseRepository
    {
        Task<List<Phase>> CreateAsync(List<Phase> phases);

        Task<List<Phase>> GetAllByTaskIdAsync(Guid id);

        Task<Phase?> UpdateAsync(Guid id, Phase phase);

        Task<Phase?> DeleteAsync(Guid id);
    }
}
