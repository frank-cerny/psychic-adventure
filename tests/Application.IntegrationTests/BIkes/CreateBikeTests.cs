using bike_selling_app.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using bike_selling_app.Application.Bikes.Commands;
using bike_selling_app.Application.Common.Exceptions;
using System.Linq;
using System.Globalization;

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
            command.bike.DatePurchased = "07-11-2021";
            FluentActions.Invoking(() => SendAsync(command)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateBike()
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
                    DatePurchased = "08-07-2021"
                }
            };
            var newBike = await SendAsync(command);
            newBike.SerialNumber.Should().Be("12345");
            newBike.Make.Should().Be("Miyata");
            newBike.Model.Should().Be("SuperDuty");
            newBike.PurchasePrice.Should().Be(65.78);
            newBike.PurchasedFrom.Should().Be("Facebook Marketplace");
            CultureInfo culture = new CultureInfo("en-US");
            newBike.DatePurchased.ToShortDateString().Should().Be("8/7/2021");
        }
    }
}