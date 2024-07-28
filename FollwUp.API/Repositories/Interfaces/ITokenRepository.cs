using Microsoft.AspNetCore.Identity;

namespace FollwUp.API.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user);
    }
}
