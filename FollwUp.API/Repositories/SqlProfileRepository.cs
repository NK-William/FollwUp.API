using FollwUp.API.Data;
using FollwUp.API.Model.Domain;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FollwUp.API.Repositories
{
    public class SqlProfileRepository : IProfileRepository
    {
        private readonly FollwupDbContext dbContext;

        public SqlProfileRepository(FollwupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Profile> CreateAsync(Profile profile)
        {
            await dbContext.Profiles.AddAsync(profile);
            await dbContext.SaveChangesAsync();
            return profile;
        }
    }
}
