using bike_selling_app.Infrastructure.Persistence;
using bike_selling_app.WebUI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using bike_selling_app.Application.Common.Interfaces;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static IApplicationDbContext _context; 

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        // Remove database from past run
        if (File.Exists("testrun.log"))
        {
            File.Delete("testrun.log");
        }

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        // This allows us to use dependency injection within the tests
        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "bike_selling_app.WebUI"));

        // Configure logging for integration tests
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("testrun.log")
            .CreateLogger();

        services.AddLogging(config => {
            config.AddSerilog();
        });

        startup.ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();

        _scopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
        _context = serviceProvider.GetService<IApplicationDbContext>();

        EnsureDatabase();
    }

    public static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Database.Migrate();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static Task<TEntity> CallContextMethod<TEntity>(string methodName, params object[] args) where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        var method = context.GetType().GetMethod(methodName);
        return method.Invoke(context, args) as Task<TEntity>;
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
