using FollwUp.API.Model.Domain;

namespace FollwUp.API.Repositories.Interfaces
{
    public interface IPhaseRepository
    {
        Task<List<Phase>> CreateAsync(List<Phase> phases);
    }
}
