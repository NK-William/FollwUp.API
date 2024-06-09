namespace FollwUp.API.Repositories.Interfaces
{
    public interface IProfileRepository
    {
        Task<Model.Domain.Profile> CreateAsync(Model.Domain.Profile profile);
    }
}
