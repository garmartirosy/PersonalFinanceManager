using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Personal_Finance_Manager.Data
{
    public class UserRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public UserRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext context,
            UserManager<IdentityUser> userManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != null)
                {
                    var user = await userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        var roles = await userManager.GetRolesAsync(user);

                        if (roles.Count == 0)
                        {
                            await userManager.AddToRoleAsync(user, IdentitySeed.UserRole);
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
