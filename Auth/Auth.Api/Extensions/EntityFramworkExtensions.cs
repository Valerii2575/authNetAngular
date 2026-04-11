using Auth.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Extensions
{
    public static class EntityFramworkExtensions
    {
        public static IServiceCollection AddInjectDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
            return services;
        }

    }
}
