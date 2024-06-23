using FollwUp.API.Data;
using FollwUp.API.Model.Domain;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FollwUp.API.Repositories
{
    public class SqlPhaseRepository : IPhaseRepository
    {
        private readonly FollwupDbContext dbContext;
        public SqlPhaseRepository(FollwupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Phase>> CreateAsync(List<Phase> phases)
        {
            await dbContext.Phases.AddRangeAsync(phases);
            await dbContext.SaveChangesAsync();
            return phases;
        }

        public async Task<List<Phase>> GetAllByTaskIdAsync(Guid id)
        {
            return await dbContext.Phases.Where(p => p.TaskId == id).ToListAsync();
        }
    }
}
