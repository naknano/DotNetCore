using Microsoft.AspNetCore.Identity;

namespace _APP2.Repository
{
    public interface ITokenRepositoryl
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);

    }
}
