namespace FollwUp.API.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<Model.Domain.Task> CreateAsync(Model.Domain.Task task);
    }
}
