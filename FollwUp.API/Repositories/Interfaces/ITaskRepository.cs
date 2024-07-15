namespace FollwUp.API.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<Model.Domain.Task> CreateAsync(Model.Domain.Task task);
        Task<Model.Domain.Task?> GetByIdAsync(Guid id);
        Task<Model.Domain.Task?> DeleteAsync(Guid id);
        Task<Model.Domain.Task?> UpdateAsync(Guid id, Model.Domain.Task task);
    }
}
