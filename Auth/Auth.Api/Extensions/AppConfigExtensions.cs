using Auth.Api.Models;

namespace Auth.Api.Extensions
{
    public static class AppConfigExtensions
    {
        public static WebApplication UseConfigureCORS(this WebApplication app)
        {
            app.UseCors(options =>
            {
                options.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
            return app;
        }

        public static IServiceCollection AddAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            return services;
        }
    }
}
