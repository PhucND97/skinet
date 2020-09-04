using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindUserByClaimPrincipalWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return await userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindByEmailFromClaimPrincipal(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            return await userManager.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}