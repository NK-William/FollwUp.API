﻿using FollwUp.API.Model.Domain;

namespace FollwUp.API.Repositories.Interfaces
{
    public interface IInvitationRepository
    {
        Task<Invitation> CreateAsync(Invitation invitation);
        Task<Invitation?> GetByTaskIdAsync(Guid id);
        Task<List<Invitation>> GetAllByPhoneNumber(string phoneNumber);
        Task<Invitation?> DeleteAsync(Guid id);
    }
}
