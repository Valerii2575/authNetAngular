using Auth.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.Api.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services) 
        {
            services.AddIdentityApiEndpoints<AppUser>()
                    .AddEntityFrameworkStores<AppDbContext>();
            return services;
        }

        public static IServiceCollection AddConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            return services;
        }

        public static IServiceCollection AddIdentityAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme =
                x.DefaultChallengeScheme =
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                //x.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                //x.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                //x.DefaultForbidScheme = IdentityConstants.ApplicationScheme;
                //x.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
            })
            .AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:JWTSecret"]))
                    //    ValidateIssuer = false,
                    //ValidateAudience = false,
                    //ValidateLifetime = true,
                    //ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    //ValidAudience = builder.Configuration["Jwt:Audience"],
                };
            });
            return services;
        }
    }
}
