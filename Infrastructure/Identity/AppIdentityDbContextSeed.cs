using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!userManager.Users.Any())
                {
                    var user = new AppUser
                    {
                        DisplayName = "Bob",
                        Email = "bob@test.com",
                        UserName = "bob@test.com",
                        Address = new Address
                        {
                            FirstName = "Bob",
                            LastName = "France",
                            Street = "10 the Street",
                            City = "New York",
                            State = "NY",
                            Zipcode = "90210"
                        },

                    };

                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AppIdentityDbContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}