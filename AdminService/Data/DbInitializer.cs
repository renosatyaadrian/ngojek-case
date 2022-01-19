using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AdminService.Data
{
    public class DbInitializer
    {
        private ApplicationDbContext _context;

        public DbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Initialize(UserManager<IdentityUser> userManager)
        {
            var roleStore = new RoleStore<IdentityRole>(_context);

            if (userManager.FindByEmailAsync("admin@gmail.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com"
                };

                if (!_context.Roles.Any(r => r.Name == "Admin"))
                {
                    await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                }

                await _context.SaveChangesAsync();

                IdentityResult result = userManager.CreateAsync(user, "Admin@123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                    userManager.SetLockoutEnabledAsync(user, false).Wait();
                }
            }

        }
    }
}
