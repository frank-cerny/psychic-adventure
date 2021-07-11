using bike_selling_app.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using bike_selling_app.Application.Bikes.Commands;
using bike_selling_app.Application.Common.Exceptions;

namespace bike_selling_app.Application.IntegrationTests.Bikes.Commands
{
    using static Testing;
    public class CreateBikeTests : TestBase
    {
        [Test]
        public async Task ShouldRequireValidDateString()
        {
            var command = new CreateBikeCommand
            {
                bike = new BikeRequestDto
                {
                    SerialNumber = "12345",
                    Make = "Miyata",
                    Model = "SuperDuty",
                    PurchasePrice = 65.78,
                    PurchasedFrom = "Facebook Marketplace",
                    DatePurchased = "1234543984180499038210"
                }
            };
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
            command.bike.DatePurchased = "89chfkjhae";
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
            // Todo add a specific format of dates that we approve of so we can validate!
            command.bike.DatePurchased = "7112021";
        }

        [Test]
        public async Task ShouldCreateBike()
        {

        }
    }
}