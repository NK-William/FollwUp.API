using FollwUp.API.Data;
using FollwUp.API.Model.Domain;
using FollwUp.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FollwUp.API.Repositories
{
    public class SqlInvitationRepository : IInvitationRepository
    {
        private readonly FollwupDbContext dbContext;

        public SqlInvitationRepository(FollwupDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Invitation> CreateAsync(Invitation invitation)
        {
            await dbContext.Invitations.AddAsync(invitation);
            await dbContext.SaveChangesAsync();
            return invitation;
        }

        public async Task<List<Invitation>> GetAllByPhoneNumber(string phoneNumber)
        {
            return await dbContext.Invitations.Where(i => i.PhoneNumber == phoneNumber).ToListAsync();
        }

        public async Task<Invitation?> GetByTaskIdAsync(Guid id)
        {
            return await dbContext.Invitations.FirstOrDefaultAsync(i => i.TaskId == id);
        }

        public async Task<Invitation?> DeleteAsync(Guid id)
        {
            var existingInvitation = await dbContext.Invitations.FirstOrDefaultAsync(i => i.Id == id);
            if (existingInvitation == null)
                return null;

            dbContext.Invitations.Remove(existingInvitation);
            await dbContext.SaveChangesAsync();
            return existingInvitation;
        }
    }
}
