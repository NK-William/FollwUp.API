using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FollwUp.API.Data
{
    public class FollwupAuthDbContext : IdentityDbContext
    {
        public FollwupAuthDbContext(DbContextOptions<FollwupAuthDbContext> options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);

        //    var readerRoleId = "9b751273-d980-494f-a7b3-edfeb91dcf33";
        //    var writerRoleId = "6c1e5234-351b-4338-9598-2e15ba7c0226";

        //    var roles = new List<IdentityRole>
        // {
        //     new IdentityRole
        //     {
        //         Id = readerRoleId,
        //         ConcurrencyStamp= readerRoleId,
        //         Name = "Reader",
        //         NormalizedName = "Reader".ToUpper()
        //     },
        //     new IdentityRole
        //     {
        //         Id = writerRoleId,
        //         ConcurrencyStamp= writerRoleId,
        //         Name = "Writer",
        //         NormalizedName = "Writer".ToUpper()
        //     },
        // };

        //    builder.Entity<IdentityRole>().HasData(roles);
        //}
    }
}
