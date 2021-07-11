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
            if (configuration.GetValue<bool>("UseSqlServer"))
            {
                services.AddDbContext<ApplicationDbContext, ApplicationDbContextSqlServer>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContextSqlServer).Assembly.FullName)));
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}