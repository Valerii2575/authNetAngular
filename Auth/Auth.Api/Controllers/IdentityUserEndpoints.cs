using Auth.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Auth.Api.Controllers
{
    public class UserRegistrationModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginnModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public static class IdentityUserEndpoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder app, IConfiguration configuration)
        {
            app.MapPost("/signup", CreateUser);

            app.MapPost("/signin", Signin);

            return app;
        }

        private static async Task<IResult> CreateUser(UserManager<AppUser> userManager,
                        [FromBody] UserRegistrationModel model)
        {
            AppUser user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Results.Ok(new { Message = "User registered successfully" });
            }
            else
            {
                return Results.BadRequest(string.Join(";\n", result.Errors.Select(x => x.Description).ToList()));
            }
        }

        private static async Task<IResult> Signin(UserManager<AppUser> userManager,
                [FromBody] LoginnModel loginModel, IOptions<AppSettings> appSettings)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Results.Ok(new { Token = token });
            }
            else
            {
                return Results.BadRequest(new { message = "User or Password is incorrect" });
            }
        }
    }
}
