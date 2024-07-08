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

        public async Task<Phase?> UpdateAsync(Guid id, Phase phase)
        {
            var existingPhase = await dbContext.Phases.FirstOrDefaultAsync(p => p.id == id);

            if (existingPhase == null)
                return null;

            existingPhase.Name = phase.Name;
            existingPhase.Number = phase.Number;
            existingPhase.Description = phase.Description;
            existingPhase.Icon = phase.Icon;
            existingPhase.Status = phase.Status;
            existingPhase.TaskId = phase.TaskId;

            await dbContext.SaveChangesAsync();

            return existingPhase;
        }


        public async Task<Phase?> DeleteAsync(Guid id)
        {
            var existingPhase = await dbContext.Phases.FirstOrDefaultAsync(p => p.id == id);

            if(existingPhase == null)
                return null;

            dbContext.Phases.Remove(existingPhase);
            await dbContext.SaveChangesAsync();
            return existingPhase;
        }
    }
}
