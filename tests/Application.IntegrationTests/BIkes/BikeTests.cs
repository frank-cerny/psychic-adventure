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
        // Create Bike Tests
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

        // Delete Bike Tests
        [Test]
        public async Task ShouldRequireValidIdToDelete()
        {
            var createCommand = new CreateBikeCommand
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
            var newBike = await SendAsync(createCommand);
            var deleteCommand = new DeleteBikeCommand
            {
                bikeId = -1
            };
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().Throw<ValidationException>();
            deleteCommand.bikeId = newBike.Id;
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldDeleteBike()
        {
            var createCommand = new CreateBikeCommand
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
            var newBike = await SendAsync(createCommand);
            newBike.SerialNumber.Should().Be("12345");
            var deleteCommand = new DeleteBikeCommand
            {
                bikeId = newBike.Id
            };
            var deletedBike = await SendAsync<Bike>(deleteCommand);
            deletedBike.SerialNumber.Should().Be("12345");
            deletedBike.Model.Should().Be("SuperDuty");
            // On second call to delete, there should be a validation exception because it no longer exists
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().Throw<ValidationException>();
        }

        // Update Bike Tests
        [Test]
        public async Task ShouldRequireValidIdOnUpdate()
        {
            var createCommand = new CreateBikeCommand
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
            var newBike = await SendAsync(createCommand);
            var updateCommand = new UpdateBikeCommand
            {
                bike = new BikeRequestDto
                {
                    SerialNumber = "12345",
                    Make = "Miyata",
                    Model = "F150",
                    PurchasePrice = 72.25,
                    PurchasedFrom = "Facebook Marketplace",
                    DatePurchased = "08-07-2021"
                },
                bikeId = -1
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.bikeId = newBike.Id;
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireNonNullEntity()
        {
            var createCommand = new CreateBikeCommand
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
            var newBike = await SendAsync(createCommand);
            var updateCommand = new UpdateBikeCommand
            {
                // The make is mising from the request DTO, so the call should fail
                bike = new BikeRequestDto
                {
                    SerialNumber = "12345",
                    Model = "F150",
                    PurchasePrice = 72.25,
                    PurchasedFrom = "Facebook Marketplace",
                    DatePurchased = "08-07-2021"
                },
                bikeId = newBike.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.bike.Make = "Ross";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireValidDateStringOnUpdate()
        {
            var createCommand = new CreateBikeCommand
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
            var newBike = await SendAsync(createCommand);
            var updateCommand = new UpdateBikeCommand
            {
                // The make is mising from the request DTO, so the call should fail
                bike = new BikeRequestDto
                {
                    SerialNumber = "12345",
                    Make = "Ross",
                    Model = "F150",
                    PurchasePrice = 72.25,
                    PurchasedFrom = "Facebook Marketplace",
                    DatePurchased = "4374987198347"
                },
                bikeId = newBike.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.bike.DatePurchased = "05-25-2019";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldUpdateBike()
        {
            var createCommand = new CreateBikeCommand
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
            var newBike = await SendAsync(createCommand);
            var updateCommand = new UpdateBikeCommand
            {
                // The make is mising from the request DTO, so the call should fail
                bike = new BikeRequestDto
                {
                    SerialNumber = "54321",
                    Make = "Canyon",
                    Model = "Diverge",
                    PurchasePrice = 165.99,
                    PurchasedFrom = "Ebay",
                    DatePurchased = "05-25-2019"
                },
                bikeId = newBike.Id
            };
            var updatedBike = await SendAsync(updateCommand);
            // This proves the update command returns a bike entity
            updatedBike.Make.Should().Be("Canyon");
            var allBikes = await CallContextMethod<IList<Bike>>("GetAllBikes");
            updatedBike = allBikes.SingleOrDefault(b => b.Id == updatedBike.Id);
            // Get updated bike from database as well to validate changes took hold
            updatedBike.Make.Should().Be("Canyon");
            updatedBike.Model.Should().Be("Diverge");
            updatedBike.PurchasePrice.Should().Be(165.99);
            updatedBike.PurchasedFrom.Should().Be("Ebay");
            updatedBike.SerialNumber.Should().Be("54321");
            CultureInfo culture = new CultureInfo("en-US");
            updatedBike.DatePurchased.ToShortDateString().Should().Be("5/25/2019");
        }
    }
}