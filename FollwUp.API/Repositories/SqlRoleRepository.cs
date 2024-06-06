using FollwUp.API.Data;
using FollwUp.API.Model.Domain;
using FollwUp.API.Repositories.Interfaces;

namespace FollwUp.API.Repositories
{
    public class SqlRoleRepository : IRoleRepository
    {
        private readonly FollwupDbContext context;

        public SqlRoleRepository(FollwupDbContext context)
        {
            this.context = context;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            context.Roles.Add(role);
            await context.SaveChangesAsync();
            return role;
        }
    }
}
