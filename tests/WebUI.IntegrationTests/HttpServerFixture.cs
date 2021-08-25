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

namespace bike_selling_app.WebUI.IntegrationTests
{
    public sealed class HttpServerFixture : WebApplicationFactory<Program>, ITestOutputHelperAccessor, IDisposable
    {
        public HttpServerFixture() : base()
        {

        }
        public ITestOutputHelper OutputHelper { get; set; }
        protected override IHostBuilder CreateHostBuilder()
        {
            // This calls Program.CreateWebHostBuilder()
            var builder = base.CreateHostBuilder();
            builder.ConfigureLogging(logging => {
                logging.AddXUnit(OutputHelper);
                logging.SetMinimumLevel(LogLevel.Trace);
            });
            return builder;
        }
    }
}