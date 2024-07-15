using FollwUp.API.Data;
using FollwUp.API.Enums;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain = FollwUp.API.Model.Domain;

namespace FollwUp.API.Repositories
{
    public class SqlTaskRepository : ITaskRepository
    {
        private readonly FollwupDbContext dbContext;

        public SqlTaskRepository(FollwupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Domain.Task> CreateAsync(Domain.Task task)
        {
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
            return task;
        }

        public async Task<Domain.Task?> DeleteAsync(Guid id)
        {
            var existingTask = await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (existingTask == null)
                return null;

            dbContext.Tasks.Remove(existingTask);
            await dbContext.SaveChangesAsync();
            return existingTask;
        }

        public async Task<Domain.Task?> GetByIdAsync(Guid id)
        {
            return await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Domain.Task?> UpdateAsync(Guid id, Domain.Task task)
        {
            var existingTask = dbContext.Tasks.FirstOrDefault(t => t.Id == id);

            if(existingTask == null)
                return null;

            existingTask.Name = task.Name;
            existingTask.ProgressToHundred = task.ProgressToHundred;
            existingTask.Organization = task.Organization;
            existingTask.Eta = task.Eta;
            existingTask.Color = task.Color;
            existingTask.Status = Enums.TaskStatus.Accepted;
            existingTask.Description = task.Description;

            await dbContext.SaveChangesAsync();

            return existingTask;

        }
    }
}
