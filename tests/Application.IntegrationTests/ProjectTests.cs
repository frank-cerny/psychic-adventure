using bike_selling_app.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using bike_selling_app.Application.Projects.Commands;
using bike_selling_app.Application.Bikes.Commands;
using bike_selling_app.Application.Common.Exceptions;
using System.Linq;
using System.Globalization;

namespace bike_selling_app.Application.IntegrationTests.Bikes
{
    using static Testing;
    public class ProjectTests : TestBase
    {
        [Test]
        public async Task ShouldRequireValidDateString()
        {
            var command = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {

                }
            };
            // Validate both dates being null
            FluentActions.Invoking(() => SendAsync(command)).Should().NotThrow<ValidationException>();
            // Validate only start date being non-null
            command.project.DateStarted = "2020-09-08";
            FluentActions.Invoking(() => SendAsync(command)).Should().NotThrow<ValidationException>();
            // Validate invalid start date
            command.project.DateStarted = "fjdhajkfsf";
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
            command.project.DateStarted = "2020-07-21";
            // Validate invalid end date
            command.project.DateEnded = "22001-09-29";
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
            // Date ended must be the same or after date started
            command.project.DateEnded = "2020-07-19";
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
            // Both valid dates
            command.project.DateEnded = "2020-09-10";
            FluentActions.Invoking(() => SendAsync(command)).Should().NotThrow<ValidationException>();
            // Validate that a null start date and non-null end date is invalid
            command.project.DateStarted = null;
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireValidBikeIds()
        {
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    BikeIds = new List<int>() { -1 }
                }
            };
            FluentActions.Invoking(() => SendAsync(projectCommand)).Should().Throw<ValidationException>();
            // Create a bike (using the bike command) and get its valid ID
            var bikeCommand = new CreateBikeCommand
            {
                bike = new BikeRequestDto
                {
                    SerialNumber = "32781937298",
                    DatePurchased = "2020-08-12",
                    Make = "Schwinn",
                    Model = "Starlet II",
                    PurchasedFrom = "Facebook Marketplace"
                }
            };
            var newBike = await SendAsync<Bike>(bikeCommand);
            projectCommand.project.BikeIds = new List<int>() { newBike.Id };
            FluentActions.Invoking(() => SendAsync(projectCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateProjectWithoutBikes()
        {

        }

        [Test]
        public async Task ShouldCreateProjectWithBikes()
        {
            // Create 2 bikes to add to the project creation
        }
    }
}