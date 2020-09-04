using Core.Entities.Identities;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}