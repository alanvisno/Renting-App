using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestCore.Security.Identity;

namespace TestCore.Api.Services
{
    public class AppInitializer : IAppInitializer
    {
        public void SetUserDataMapping(WebApplication app)
        {
            app.MapGet("api/Identity/me", (IHttpContextAccessor contextAccessor) =>
            {
                var user = contextAccessor.HttpContext.User;

                return Results.Ok(new
                {
                    Claims = user.Claims.Select(s => new
                    {
                        s.Type,
                        s.Value
                    }).ToList(),
                    user.Identity.Name,
                    user.Identity.IsAuthenticated,
                    user.Identity.AuthenticationType
                });
            })
            .RequireAuthorization();
        }

        public void SetTokenMapping(WebApplication app, WebApplicationBuilder builder)
        {
            app.MapPost("api/Identity/token", async (AuthenticateRequest request, UserManager<User> userManager) =>
            {
                var user = await userManager.FindByNameAsync(request.UserName);

                if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                {
                    return Results.Forbid();
                }

                var roles = await userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                var tokenDescriptor = new JwtSecurityToken(
                    issuer: builder.Configuration["Jwt:Issuer"],
                    audience: builder.Configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);

                var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

                return Results.Ok(new
                {
                    AccessToken = jwt
                });
            });
        }

        public async Task Initialize(WebApplication app)
        {
            var scopeFactory = app!.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<IdentityDataContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            context.Database.EnsureCreated();

            if (!userManager.Users.Any())
            {
                logger.LogInformation("Creando usuario de prueba");

                var newUser = new User
                {
                    Email = "test@demo.com",
                    FirstName = "Alan",
                    LastName = "Visno",
                    UserName = "Admin"
                };

                await userManager.CreateAsync(newUser, "Admin_3792");
            }
        }
    }
}
