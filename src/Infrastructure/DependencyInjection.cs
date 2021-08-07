using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Infrastructure.Persistence;
using bike_selling_app.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace bike_selling_app.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseSqlite"))
            {
                services.AddDbContext<ApplicationDbContext, ApplicationDbContextSqlite>(options =>
                    options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            }
            // Reference: https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql
            if (configuration.GetValue<bool>("UseMariaDb"))
            {
                // TODO - Replace with actual MariaDB Version
                var serverVersion = new MySqlServerVersion(new System.Version(8, 0, 25));

                services.AddDbContext<ApplicationDbContext, ApplicationDbContextMySql>(options =>
                    options.UseMySql(configuration.GetConnectionString("DefaultConnection"), serverVersion)
                    .EnableSensitiveDataLogging()
                    .EnableSensitiveDataLogging()
                );
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}