using FollwUp.API.Data;
using FollwUp.API.Repositories.Interfaces;
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
    }
}
