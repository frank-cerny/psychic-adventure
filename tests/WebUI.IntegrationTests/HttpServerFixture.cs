using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using bike_selling_app.Application.Common.Interfaces;
using Xunit.Abstractions;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Hosting;
using bike_selling_app.Infrastructure.Persistence;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace bike_selling_app.WebUI.IntegrationTests
{
    public sealed class HttpServerFixture : WebApplicationFactory<Startup>, ITestOutputHelperAccessor, IDisposable 
    {
        private WebApplicationFactory<Startup> _factory { get; set; }
        private ApplicationDbContext _context { get; set; }
        public WebApplicationFactory<Startup> Factory { 
            get {
                return _factory ?? ConfigureWebApplicationFactory();
            }
        }
        public ITestOutputHelper OutputHelper { get; set; }
        private IConfiguration _config { get; set; }
        private readonly string _hostingEnvironment = "IntegrationTests";
        public HttpServerFixture() : base()
        {
            _factory = ConfigureWebApplicationFactory();
        }

        public void ConfigureTests()
        {
            
        }

        private WebApplicationFactory<Startup> ConfigureWebApplicationFactory()
        {
            // TODO - When is this called?
            return new WebApplicationFactory<Startup>().WithWebHostBuilder(builder => {
                // Pretty sure this gets called after base.
                builder.UseEnvironment(_hostingEnvironment);
                builder.ConfigureLogging(logging => {
                    logging.AddXUnit();
                });
                // Reference: https://gunnarpeipman.com/aspnet-core-integration-tests-appsettings/
                builder.ConfigureAppConfiguration((context, conf) => {
                    conf.AddJsonFile("appsettings.Integration.json");
                });
                ConfigureWebHost(builder);
            });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var sp = services.BuildServiceProvider();

                _config = sp.GetRequiredService<IConfiguration>();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    _context = db;
                    var logger = scopedServices
                        .GetRequiredService<ILogger<WebApplicationFactory<Startup>>>();

                    // The dispose method in the fixture should remove the database after each run, but just in case
                    var databasePathConfig = _config.GetValue<string>("ConnectionStrings:DefaultConnection");
                    // Format of the config is: Filename=web-integration-tests.db
                    var databasePath = databasePathConfig.Split("=")[1];

                    // db.Database.Migrate();
                    db.Database.EnsureCreated();

                    try
                    {
                        // Clear the database before re-seeding it (so that all tests have the same base)
                        // I could have deleted the database, but I could not figure out how to re-create without error
                        Utilities.Clear(db);
                        Utilities.SeedDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                             "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }

        public void ResetDb()
        {
            var serviceProvider = _factory.Services;
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            Utilities.Clear(context);
            Utilities.SeedDbForTests(context);
        }

        // TODO - How can we ensure this runs?
        // public void Dispose()
        // {
        //     if (this.Factory != null)
        //     {
        //         this.Factory.Dispose();
        //     }
        //     // Remove testing database using IConfiguration (passed in at Startup)
        //     var databasePath = _config.GetValue<string>("ConnectionStrings:DefaultConnection");
        //     if (File.Exists(databasePath))
        //     {
        //         File.Delete(databasePath);
        //     }
        // }
    }
}