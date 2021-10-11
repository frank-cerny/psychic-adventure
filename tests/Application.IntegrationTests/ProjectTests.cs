using bike_selling_app.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using bike_selling_app.Application.Projects.Commands;
using bike_selling_app.Application.Bikes.Commands;
using bike_selling_app.Application.NonCapitalItems.Commands;
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
                    Title = "testing-1"
                }
            };
            // Validate both dates being null
            FluentActions.Invoking(() => SendAsync(command)).Should().NotThrow<ValidationException>();
            // Validate only start date being non-null
            command.project.DateStarted = "2020-09-08";
            command.project.Title = "testing-11";
            FluentActions.Invoking(() => SendAsync(command)).Should().NotThrow<ValidationException>();
            // Validate invalid start date
            command.project.DateStarted = "fjdhajkfsf";
            command.project.Title = "testing-111";
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
            command.project.Title = "testing-1111";
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
                    Title = "testing1",
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
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing2",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12"
                }
            };
            var result = await SendAsync(projectCommand);
            // Get the project from the database to validate it was stored correctly
            var newProject = await CallContextMethod<Project>("GetProjectById", result.Id);
            newProject.Description.Should().Be("A simple project!");
            CultureInfo culture = new CultureInfo("en-US");
            newProject.DateStarted.Value.ToShortDateString().Should().Be("9/15/2020");
            newProject.DateEnded.Value.ToShortDateString().Should().Be("10/12/2020");
        }

        [Test]
        public async Task ShouldFilterOnDuplicateBikeIdsOnCreate()
        {
            // Create 1 bike and attempt to add it twice (should only be added once)
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
            var newBike1 = await SendAsync<Bike>(bikeCommand);
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing3",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                    BikeIds = new List<int>() { newBike1.Id, newBike1.Id }
                }
            };
            var result = await SendAsync(projectCommand);
            result.Bikes.Should().HaveCount(1);
        }

        [Test]
        public async Task ShouldCreateProjectWithBikes()
        {
            // Create 2 bikes to add to the project creation
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
            var newBike1 = await SendAsync<Bike>(bikeCommand);
            bikeCommand = new CreateBikeCommand
            {
                bike = new BikeRequestDto
                {
                    SerialNumber = "231222-32F",
                    DatePurchased = "2020-07-16",
                    Make = "Huffy",
                    Model = "Sportsman",
                    PurchasedFrom = "Ebay"
                }
            };
            var newBike2 = await SendAsync<Bike>(bikeCommand);
            // Create a non capital item
            var nonCapitalItemCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 4.45,
                    UnitsPurchased = 3,
                    DatePurchased = "06-23-2015"
                }
            };
            var nonCapitalItem = await SendAsync(nonCapitalItemCommand);
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing3",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                    BikeIds = new List<int>() { newBike1.Id, newBike2.Id },
                    NonCapitalItemIds = new List<int>() { nonCapitalItem.Id }
                }
            };
            var result = await SendAsync(projectCommand);
            // Get the project from the database to validate it was stored correctly
            var newProject = await CallContextMethod<Project>("GetProjectById", result.Id);
            newProject.Description.Should().Be("A simple project!");
            CultureInfo culture = new CultureInfo("en-US");
            newProject.DateStarted.Value.ToShortDateString().Should().Be("9/15/2020");
            newProject.DateEnded.Value.ToShortDateString().Should().Be("10/12/2020");
            newProject.Bikes.Select(b => b.Id).ToList().Should().Contain(newBike1.Id);
            newProject.Bikes.Select(b => b.Id).ToList().Should().Contain(newBike2.Id);
            newProject.Title.Should().Be("testing3");
            newProject.NonCapitalItems.Should().HaveCount(1);
            newProject.NonCapitalItems[0].Name.Should().Be("MyItem");
        }

        [Test]
        public async Task ShouldRequireUniqueTitleOnCreate()
        {
            // Create project
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing4",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12"
                }
            };
            var result = await SendAsync(projectCommand);
            FluentActions.Invoking(() => SendAsync(projectCommand)).Should().Throw<ValidationException>();
            projectCommand.project.Title = "A different title!";
            FluentActions.Invoking(() => SendAsync(projectCommand)).Should().NotThrow<ValidationException>();
            projectCommand.project.Title = null;
            FluentActions.Invoking(() => SendAsync(projectCommand)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldFilterDuplicateChildIdsOnCreate()
        {
            var nonCapitalItemCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 4.45,
                    UnitsPurchased = 3,
                    DatePurchased = "06-23-2015"
                }
            };
            var nonCapitalItem = await SendAsync(nonCapitalItemCommand);
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing3",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                    NonCapitalItemIds = new List<int>() { nonCapitalItem.Id, nonCapitalItem.Id, nonCapitalItem.Id }
                }
            };
            var result = await SendAsync(projectCommand);
            // Validate only a single noncapital exists within the project
            var projects = await CallContextMethod<IList<Project>>("GetAllProjects");
            var newProject = projects.SingleOrDefault(p => p.Id == result.Id);
            newProject.NonCapitalItems.Should().HaveCount(1);
        }

        // Delete Tests

        [Test]
        public async Task ShouldRequireValidIdToDelete()
        {
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing2",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12"
                }
            };
            var result = await SendAsync(projectCommand);
            var deleteCommand = new DeleteProjectCommand
            {
                projectId = -1
            };
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().Throw<ValidationException>();
            deleteCommand.projectId = result.Id;
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldDeleteProject()
        {
            var nonCapitalItemCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 4.45,
                    UnitsPurchased = 3,
                    DatePurchased = "06-23-2015"
                }
            };
            var nonCapitalItem = await SendAsync(nonCapitalItemCommand);
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing2",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                    NonCapitalItemIds = new List<int>() { nonCapitalItem.Id }
                }
            };
            var result = await SendAsync(projectCommand);
            var newProject = await CallContextMethod<Project>("GetProjectById", result.Id);
            newProject.Should().NotBeNull();
            newProject.Id.Should().Be(result.Id);
            var deleteCommand = new DeleteProjectCommand
            {
                projectId = result.Id
            };
            var deletedProject = await SendAsync(deleteCommand);
            deletedProject.Title.Should().Be(result.Title);
            newProject = await CallContextMethod<Project>("GetProjectById", result.Id);
            newProject.Should().BeNull();
            // Ensure all child items are NOT deleted (they must be manually deleted)
            var allNonCapitalItems = await CallContextMethod<IList<NonCapitalItem>>("GetAllNonCapitalItems");
            var existingNonCapitalItem = allNonCapitalItems.SingleOrDefault(nci => nci.Id == nonCapitalItem.Id);
            existingNonCapitalItem.Should().NotBeNull();
        }

        // Update Tests

        [Test]
        public async Task ShouldRequireValidDateStringsOnUpdate()
        {
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing2",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12"
                }
            };
            var result = await SendAsync(projectCommand);
            var updateProjectCommand = new UpdateProjectCommand
            {
                projectId = result.Id,
                project = new ProjectRequestDto
                {
                    Title = "testing2",
                    Description = "A simple project!",
                    DateStarted = "2020111-09-15",
                    DateEnded = "2020111-10-12" 
                }
            };
            FluentActions.Invoking(() => SendAsync(updateProjectCommand)).Should().Throw<ValidationException>();
            updateProjectCommand.project.DateStarted = "2021-08-15";
            updateProjectCommand.project.DateEnded = "2021-07-15";
            FluentActions.Invoking(() => SendAsync(updateProjectCommand)).Should().Throw<ValidationException>();
            updateProjectCommand.project.DateEnded = "2021-09-15";
            FluentActions.Invoking(() => SendAsync(updateProjectCommand)).Should().NotThrow<ValidationException>();
        }

        // TODO Create/Update should have valid NonCapitalItem Ids :)

        [Test]
        public async Task ShouldRequireUniqueTitleOnUpdate()
        {
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing2",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12"
                }
            };
            var resultOne = await SendAsync(projectCommand);
            // Create a second project with a different title
            projectCommand.project.Title = "newtitle";
            var resultTwo = await SendAsync(projectCommand);
            // Now attempt to update the first project with the title from the second
            var updateProjectCommand = new UpdateProjectCommand
            {
                projectId = resultOne.Id,
                project = new ProjectRequestDto
                {
                    Title = "newtitle",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12" 
                }
            };
            FluentActions.Invoking(() => SendAsync(updateProjectCommand)).Should().Throw<ValidationException>();
            updateProjectCommand.project.Title = "AnotherNewTitle";
            FluentActions.Invoking(() => SendAsync(updateProjectCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireValidBikeIdsOnUpdate()
        {
            // Create 1 bike and attempt to add twice 
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
            var newBike1 = await SendAsync<Bike>(bikeCommand);
            // Do not add any bikes at first
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing3",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                }
            };
            var result = await SendAsync(projectCommand);
            result.Bikes.Should().HaveCount(0);
            // Now attempt to add a valid bike and invalid bike
            var updateCommand = new UpdateProjectCommand
            {
                projectId = result.Id,
                project = new ProjectRequestDto
                {
                    Title = "testing3",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                    BikeIds = new List<int>() { -1, newBike1.Id }      
                }
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.project.BikeIds = new List<int>() { newBike1.Id };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldFilterOutDuplicateBikeIdsOnUpdate()
        {
            // Create 1 bike and attempt to add twice 
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
            var newBike1 = await SendAsync<Bike>(bikeCommand);
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing3",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                    BikeIds = new List<int>() { newBike1.Id }
                }
            };
            var result = await SendAsync(projectCommand);
            result.Bikes.Should().HaveCount(1);
            // Now attempt to update with the same bike, should not add twice
            var updateCommand = new UpdateProjectCommand
            {
                projectId = result.Id,
                project = new ProjectRequestDto
                {
                    Title = "testing3",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                    BikeIds = new List<int>() { newBike1.Id }      
                }
            };
            var updateResult = await SendAsync(updateCommand);
            updateResult.Bikes.Should().HaveCount(1);
        }
        
        [Test]
        public async Task ShouldFilterDuplicateChildIdsOnUpdate()
        {
            var nonCapitalItemCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 4.45,
                    UnitsPurchased = 3,
                    DatePurchased = "06-23-2015"
                }
            };
            var nonCapitalItem = await SendAsync(nonCapitalItemCommand);
            // Do not add any bike or items at first
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing3",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                }
            };
            var result = await SendAsync(projectCommand);
            // Now update the project with the items
            var updateCommand = new UpdateProjectCommand
            {
                projectId = result.Id,
                project = new ProjectRequestDto
                {
                    Title = "newTitle",
                    Description = "A simple project!!",
                    DateStarted = "2020-09-16",
                    DateEnded = "2020-10-13",
                    NonCapitalItemIds = new List<int>() { nonCapitalItem.Id, nonCapitalItem.Id, nonCapitalItem.Id }   
                }
            };
            result = await SendAsync(updateCommand);
            // Validate only a single noncapital exists within the project
            var projects = await CallContextMethod<IList<Project>>("GetAllProjects");
            var newProject = projects.SingleOrDefault(p => p.Id == result.Id);
            newProject.NonCapitalItems.Should().HaveCount(1);
        }

        [Test]
        public async Task ShouldUpdateProject()
        {
            // Create 1 bike and attempt to add twice 
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
            var newBike1 = await SendAsync<Bike>(bikeCommand);
            // Create a non capital item
            var nonCapitalItemCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 4.45,
                    UnitsPurchased = 3,
                    DatePurchased = "06-23-2015"
                }
            };
            var nonCapitalItem = await SendAsync(nonCapitalItemCommand);
            // Do not add any bike or items at first
            var projectCommand = new CreateProjectCommand
            {
                project = new ProjectRequestDto
                {
                    Title = "testing3",
                    Description = "A simple project!",
                    DateStarted = "2020-09-15",
                    DateEnded = "2020-10-12",
                }
            };
            var result = await SendAsync(projectCommand);
            result.Bikes.Should().HaveCount(0);
            result.Title.Should().Be("testing3");
            result.Description.Should().Be("A simple project!");
            // Now attempt to add a valid bike and updated information
            var updateCommand = new UpdateProjectCommand
            {
                projectId = result.Id,
                project = new ProjectRequestDto
                {
                    Title = "newTitle",
                    Description = "A simple project!!",
                    DateStarted = "2020-09-16",
                    DateEnded = "2020-10-13",
                    BikeIds = new List<int>() { newBike1.Id },
                    NonCapitalItemIds = new List<int>() { nonCapitalItem.Id }   
                }
            };
            result = await SendAsync(updateCommand);
            result.Title.Should().Be("newTitle");
            result.Description.Should().Be("A simple project!!");
            result.DateStarted = null;
            result.DateEnded = null;
            result.Bikes.Should().HaveCount(1);
            // Also validate changes directly from the database
            var updatedProject = await CallContextMethod<Project>("GetProjectById", result.Id);
            updatedProject.Title.Should().Be("newTitle");
            updatedProject.Description.Should().Be("A simple project!!");
            updatedProject.Bikes.Should().HaveCount(1);
            updatedProject.NonCapitalItems.Should().HaveCount(1);
            updatedProject.NonCapitalItems[0].Name.Should().Be("MyItem");
        }
    }
}