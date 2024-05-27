using FollwUp.API.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace FollwUp.API.Data
{
    public class FollwupDbContext : DbContext
    {
        public FollwupDbContext(DbContextOptions<FollwupDbContext> options) : base(options)
        {
        }

        public DbSet<Model.Domain.Task> Tasks { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Phase> Phases { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
    }
}
