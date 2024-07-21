using FollwUp.API.Data;
using FollwUp.API.Model.Domain;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FollwUp.API.Repositories
{
    public class SqlRoleRepository : IRoleRepository
    {
        private readonly FollwupDbContext dbContext;

        public SqlRoleRepository(FollwupDbContext context)
        {
            this.dbContext = context;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            dbContext.Roles.Add(role);
            await dbContext.SaveChangesAsync();
            return role;
        }

        public async Task<List<Role>> GetAllByProfileIdAsync(Guid id)
        {
            return await dbContext.Roles.Where(r => r.ProfileId == id).ToListAsync();
        }

        public async Task<List<Role>> GetAllByTaskIdAsync(Guid id)
        {
            return await dbContext.Roles.Where(r => r.TaskId == id).ToListAsync();
        }
        public async Task<Role?> DeleteAsync(Guid id)
        {
            var existingRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (existingRole == null)
                return null;

            dbContext.Roles.Remove(existingRole);
            await dbContext.SaveChangesAsync();
            return existingRole;
        }
    }
}
