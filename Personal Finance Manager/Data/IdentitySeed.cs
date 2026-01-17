using Microsoft.AspNetCore.Identity;

namespace Personal_Finance_Manager.Data
{
    public static class IdentitySeed
    {
        public const string AdminRole = "Admin";
        public const string UserRole = "User";

        public static async Task Seed(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await EnsureRole(roleManager, AdminRole);
            await EnsureRole(roleManager, UserRole);

            string adminEmail = "admin@admin.admin";
            string adminPassword = "Password11!!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception("Could not create the admin: " + errors);
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, AdminRole))
            {
                await userManager.AddToRoleAsync(adminUser, AdminRole);
            }

            if (!await userManager.IsInRoleAsync(adminUser, UserRole))
            {
                await userManager.AddToRoleAsync(adminUser, UserRole);
            }
        }

        private static async Task EnsureRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                if (!result.Succeeded)
                {
                    throw new Exception($"Could not create role '{roleName}'");
                }
            }
        }
    }
}
