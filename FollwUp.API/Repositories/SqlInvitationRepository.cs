﻿using FollwUp.API.Data;
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

        public async Task<Invitation?> GetByTaskIdAsync(Guid id)
        {
            return await dbContext.Invitations.FirstOrDefaultAsync(i => i.TaskId == id);
        }
    }
}
