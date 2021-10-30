using bike_selling_app.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using bike_selling_app.Application.CapitalItems.Commands;
using bike_selling_app.Application.ExpenseItems.Commands;
using bike_selling_app.Application.Common.Exceptions;
using System.Linq;
using System.Globalization;
using System;

namespace bike_selling_app.Application.IntegrationTests.ExpenseItems
{
    using static Testing;
    public class CapitalItemTests : TestBase
    {
        // Create Tests
        [Test]
        public async Task ShouldCreateCapitalItemWithoutChildren()
        {
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    Cost = 5.67,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now validate the item was created in the database
            var allCapitalItems = await CallContextMethod<IList<CapitalItem>>("GetAllCapitalItems");
            var newItem = allCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.Should().NotBeNull();
            newItem.Cost.Should().Be(5.67);
        }

        [Test]
        public async Task ShouldCreateCapitalItemWithChildren()
        {
            // Create an expense item first
            var createExpenseItemCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "Postage",
                    UnitCost = 6.78,
                    Units = 1,
                    DatePurchased = "10-11-2021"
                }
            };
            var expenseItem = await SendAsync(createExpenseItemCommand);
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                    ExpenseItemIds = new List<int>() { expenseItem.Id }
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now validate the item was created in the database
            var allCapitalItems = await CallContextMethod<IList<CapitalItem>>("GetAllCapitalItems");
            var newItem = allCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.Should().NotBeNull();
            newItem.UnitCost.Should().Be(5.67);
            newItem.UnitsPurchased.Should().Be(1);
            newItem.ExpenseItems.Should().HaveCount(1);
            newItem.ExpenseItems[0].Name.Should().Be("Postage");
        }

        [Test]
        public async Task ShouldFilterDuplicateChildIdsOnCreate()
        {
            // Create an expense item first
            var createExpenseItemCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "Postage",
                    UnitCost = 6.78,
                    Units = 1,
                    DatePurchased = "10-11-2021"
                }
            };
            var expenseItem = await SendAsync(createExpenseItemCommand);
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                    ExpenseItemIds = new List<int>() { expenseItem.Id, expenseItem.Id }
                }
            };
            var item = await SendAsync(createCommand);
            // Now lets check the database to ensure only a single expense item was added as a child
            var allCapitalItems = await CallContextMethod<IList<CapitalItem>>("GetAllCapitalItems");
            var newItem = allCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.ExpenseItems.Should().HaveCount(1);
        }

        // Ensures all values that cannot be null are checked
        [Test]
        public async Task ShouldRequireValuesOnCreate()
        {
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {

                }
            };
            // Name, UnitCost, Units, and DatePurchased are all mandatory fields (Units/UnitCost default to 0 and cannot be null by defined)
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.CapitalItem.Name = "TestItem6";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.CapitalItem.DatePurchased = "08-09-2021";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireValidDateStringOnCreate()
        {
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "078-11-2021",
                }
            };
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.CapitalItem.DatePurchased = "89chfkjhae";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.CapitalItem.DatePurchased = "07-11-2021";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireUniqueNameDateComboOnCreate()
        {
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            // Now attempt to create a second item with the same name/date values
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.CapitalItem.Name = "UpdatedName!";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireValidExpenseItemIdsOnCreate()
        {
            // Create an expense item first
            var createExpenseItemCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "Postage",
                    UnitCost = 6.78,
                    Units = 1,
                    DatePurchased = "10-11-2021"
                }
            };
            var expenseItem = await SendAsync(createExpenseItemCommand);
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                    ExpenseItemIds = new List<int>() { -1 }
                }
            };
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            // Both an empty list and a list with valid ids is valid
            createCommand.CapitalItem.ExpenseItemIds = new List<int>();
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
            createCommand.CapitalItem.ExpenseItemIds = new List<int>() { expenseItem.Id };
            // Must also change the name so that the name/date is unique among all non capital items
            createCommand.CapitalItem.Name = "AnyOtherName!";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        // Update Tests

        [Test]
        public async Task ShouldUpdateCapitalItem()
        {
            // Create an expense item first
            var createExpenseItemCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "Postage",
                    UnitCost = 6.78,
                    Units = 1,
                    DatePurchased = "10-11-2021"
                }
            };
            var expenseItem = await SendAsync(createExpenseItemCommand);
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now update the item
            var updateCommand = new UpdateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    UnitsPurchased = 5,
                    DatePurchased = "07-11-1987",
                    ExpenseItemIds = new List<int>() { expenseItem.Id }
                },
                CapitalItemId = item.Id
            };
            var updatedItem = await SendAsync(updateCommand);
            updatedItem.Name.Should().Be("UpdatedItem!");
            // Now validate changes in the database
            var allCapitalItems = await CallContextMethod<IList<CapitalItem>>("GetAllCapitalItems");
            var newItem = allCapitalItems.SingleOrDefault(item => item.Name.Equals("UpdatedItem!"));
            newItem.Should().NotBeNull();
            newItem.UnitCost.Should().Be(4.45);
            newItem.UnitsPurchased.Should().Be(5);
            newItem.ExpenseItems.Should().HaveCount(1);
        }

        // Ensures all values that cannot be null are checked
        [Test]
        public async Task ShouldRequireValuesOnUpdate()
        {
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            // Name, UnitCost, Units, and DatePurchased are all mandatory fields (Units/UnitCost default to 0 and cannot be null by defined)
            var updateCommand = new UpdateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {

                },
                CapitalItemId = item.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.CapitalItem.Name = "TestItem6";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.CapitalItem.DatePurchased = "08-09-2021";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldFilterOutDuplicateChildIdsOnUpdate()
        {
            // Create an expense item first
            var createExpenseItemCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "Postage",
                    UnitCost = 6.78,
                    Units = 1,
                    DatePurchased = "10-11-2021"
                }
            };
            var expenseItem = await SendAsync(createExpenseItemCommand);
            // Create a second expense item (this will help confirm an update can remove expense items as well)
            createExpenseItemCommand.ExpenseItem = new ExpenseItemRequestDto
            {
                Name = "Tape!",
                UnitCost = 6.78,
                Units = 1,
                DatePurchased = "10-11-2021"
            };
            var expenseItemTwo = await SendAsync(createExpenseItemCommand);
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                    ExpenseItemIds = new List<int>() { expenseItem.Id }
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            item.ExpenseItems.Should().HaveCount(1);
            // Now update the item
            var updateCommand = new UpdateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    UnitsPurchased = 5,
                    DatePurchased = "07-11-1987",
                    ExpenseItemIds = new List<int>() { expenseItemTwo.Id, expenseItemTwo.Id, expenseItemTwo.Id }
                },
                CapitalItemId = item.Id
            };
            var updatedItem = await SendAsync(updateCommand);
            // Now validate in the database that our item only has a single child
            var allCapitalItems = await CallContextMethod<IList<CapitalItem>>("GetAllCapitalItems");
            var newItem = allCapitalItems.SingleOrDefault(item => item.Name.Equals("UpdatedItem!"));
            newItem.ExpenseItems.Should().HaveCount(1);
            newItem.ExpenseItems[0].Name.Should().Be("Tape!");
        }

        [Test]
        public async Task ShouldRequireValidDateStringOnUpdate()
        {
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            // Now update the item
            var updateCommand = new UpdateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    UnitsPurchased = 5,
                    DatePurchased = "077-11-1987"
                },
                CapitalItemId = item.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.CapitalItem.DatePurchased = "89chfkjhae";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.CapitalItem.DatePurchased = "07-11-2021";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireUniqueNameDateComboOnUpdate()
        {
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            // Create a second item
            createCommand.CapitalItem = new CapitalItemRequestDto
            {
                    Name = "MyItem2",
                    UnitCost = 0.25,
                    UnitsPurchased = 25,
                    DatePurchased = "07-11-1921", 
            };
            item = await SendAsync(createCommand);
            // Now update the item and attempt to use the same name/date as the first and second (second is fine since the id matches)
            var updateCommand = new UpdateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 4.45,
                    UnitsPurchased = 5,
                    DatePurchased = "07-11-2021"
                },
                CapitalItemId = item.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.CapitalItem.Name = "MyItem2";
            updateCommand.CapitalItem.DatePurchased = "07-11-1921";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
            updateCommand.CapitalItem.Name = "UpdatedItem";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireValidExpenseItemIdsOnUpdate()
        {
            // Create an expense item first
            var createExpenseItemCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "Postage",
                    UnitCost = 6.78,
                    Units = 1,
                    DatePurchased = "10-11-2021"
                }
            };
            var expenseItem = await SendAsync(createExpenseItemCommand);
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now update the item
            var updateCommand = new UpdateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    UnitsPurchased = 5,
                    DatePurchased = "07-11-1987",
                    ExpenseItemIds = new List<int>() { -1 }
                },
                CapitalItemId = item.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.CapitalItem.ExpenseItemIds = new List<int>() { expenseItem.Id };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>(); 
        }

        [Test]
        public async Task ShouldRequireValidCapitalItemIdOnUpdate()
        {
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now update the item
            var updateCommand = new UpdateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    UnitsPurchased = 5,
                    DatePurchased = "07-11-1987",
                },
                CapitalItemId = -1
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.CapitalItemId = item.Id;
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        // Delete Tests
        [Test]
        public async Task ShouldRequireValidCapitalItemId()
        {
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            var deleteCommand = new DeleteCapitalItemCommand
            {
                CapitalItemId = -1
            };
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().Throw<ValidationException>();
            deleteCommand.CapitalItemId = item.Id;
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldDeleteCapitalItem()
        {
            // Create an expense item first
            var createExpenseItemCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "Postage",
                    UnitCost = 6.78,
                    Units = 1,
                    DatePurchased = "10-11-2021"
                }
            };
            var expenseItem = await SendAsync(createExpenseItemCommand);
            var createCommand = new CreateCapitalItemCommand
            {
                CapitalItem = new CapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    UnitsPurchased = 1,
                    DatePurchased = "07-11-2021",
                    ExpenseItemIds = new List<int>() { expenseItem.Id }
                }
            };
            var item = await SendAsync(createCommand);
            // Validate items exist in the database
            var allCapitalItems = await CallContextMethod<IList<CapitalItem>>("GetAllCapitalItems");
            var newItem = allCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.Should().NotBeNull();
            newItem.ExpenseItems.Should().HaveCount(1);
            // Now delete the non capital item and ensure children are deleted as well
            var deleteCommand = new DeleteCapitalItemCommand
            {
                CapitalItemId = item.Id
            };
            item = await SendAsync(deleteCommand);
            item.Name.Should().Be("MyItem");
            // Now validate in the database
            allCapitalItems = await CallContextMethod<IList<CapitalItem>>("GetAllCapitalItems");
            newItem = allCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.Should().BeNull();
            var allExpenseItems = await CallContextMethod<IList<ExpenseItem>>("GetAllExpenseItems");
            // Children expense items should also be deleted (since parent item has been deleted)
            var oldExpenseItem = allExpenseItems.SingleOrDefault(ei => ei.Name.Equals("Postage") && ei.DatePurchased.ToShortDateString().Equals("10/11/2021"));
            oldExpenseItem.Should().BeNull();
        }
    }
}