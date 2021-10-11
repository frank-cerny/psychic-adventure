using bike_selling_app.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using bike_selling_app.Application.NonCapitalItems.Commands;
using bike_selling_app.Application.ExpenseItems.Commands;
using bike_selling_app.Application.Common.Exceptions;
using System.Linq;
using System.Globalization;
using System;

namespace bike_selling_app.Application.IntegrationTests.ExpenseItems
{
    using static Testing;
    public class NonCapitalItemTests : TestBase
    {
        // Create Tests
        [Test]
        public async Task ShouldCreateNonCapitalItemWithoutChildren()
        {
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now validate the item was created in the database
            var allNonCapitalItems = await CallContextMethod<IList<NonCapitalItem>>("GetAllNonCapitalItems");
            var newItem = allNonCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.Should().NotBeNull();
            newItem.UnitCost.Should().Be(5.67);
            newItem.Units.Should().Be(1);
        }

        [Test]
        public async Task ShouldCreateNonCapitalItemWithChildren()
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
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ExpenseItemIds = new List<int>() { expenseItem.Id }
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now validate the item was created in the database
            var allNonCapitalItems = await CallContextMethod<IList<NonCapitalItem>>("GetAllNonCapitalItems");
            var newItem = allNonCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.Should().NotBeNull();
            newItem.UnitCost.Should().Be(5.67);
            newItem.Units.Should().Be(1);
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
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ExpenseItemIds = new List<int>() { expenseItem.Id, expenseItem.Id }
                }
            };
            var item = await SendAsync(createCommand);
            // Now lets check the database to ensure only a single expense item was added as a child
            var allNonCapitalItems = await CallContextMethod<IList<NonCapitalItem>>("GetAllNonCapitalItems");
            var newItem = allNonCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.ExpenseItems.Should().HaveCount(1);
        }

        // Ensures all values that cannot be null are checked
        [Test]
        public async Task ShouldRequireValuesOnCreate()
        {
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {

                }
            };
            // Name, UnitCost, Units, and DatePurchased are all mandatory fields
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.NonCapitalItem.Name = "TestItem6";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.NonCapitalItem.UnitCost = 4.56;
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.NonCapitalItem.Units = 3;
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.NonCapitalItem.DatePurchased = "08-09-2021";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireValidDateStringOnCreate()
        {
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "078-11-2021",
                }
            };
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.NonCapitalItem.DatePurchased = "89chfkjhae";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.NonCapitalItem.DatePurchased = "07-11-2021";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireUniqueNameDateComboOnCreate()
        {
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            // Now attempt to create a second item with the same name/date values
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.NonCapitalItem.Name = "UpdatedName!";
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
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ExpenseItemIds = new List<int>() { -1 }
                }
            };
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            // Both an empty list and a list with valid ids is valid
            createCommand.NonCapitalItem.ExpenseItemIds = new List<int>();
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
            createCommand.NonCapitalItem.ExpenseItemIds = new List<int>() { expenseItem.Id };
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        // Update Tests

        [Test]
        public async Task ShouldUpdateNonCapitalItem()
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
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now update the item
            var updateCommand = new UpdateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    Units = 5,
                    DatePurchased = "07-11-1987",
                    ExpenseItemIds = new List<int>() { expenseItem.Id }
                },
                NonCapitalItemId = item.Id
            };
            var updatedItem = await SendAsync(updateCommand);
            updatedItem.Name.Should().Be("UpdatedItem!");
            // Now validate changes in the database
            var allNonCapitalItems = await CallContextMethod<IList<NonCapitalItem>>("GetAllNonCapitalItems");
            var newItem = allNonCapitalItems.SingleOrDefault(item => item.Name.Equals("UpdatedItem!"));
            newItem.Should().NotBeNull();
            newItem.UnitCost.Should().Be(4.45);
            newItem.Units.Should().Be(5);
            newItem.ExpenseItems.Should().HaveCount(1);
        }

        // Ensures all values that cannot be null are checked
        [Test]
        public async Task ShouldRequireValuesOnUpdate()
        {
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            // Now update the item (remember that Name, UnitCost, Units, and DatePurchased are required fields)
            var updateCommand = new UpdateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {

                },
                NonCapitalItemId = item.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.NonCapitalItem.Name = "TestItem6";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            updateCommand.NonCapitalItem.UnitCost = 4.56;
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            updateCommand.NonCapitalItem.Units = 3;
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            updateCommand.NonCapitalItem.DatePurchased = "08-09-2021";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
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
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now update the item
            var updateCommand = new UpdateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    Units = 5,
                    DatePurchased = "07-11-1987",
                    ExpenseItemIds = new List<int>() { expenseItem.Id, expenseItem.Id, expenseItem.Id }
                },
                NonCapitalItemId = item.Id
            };
            var updatedItem = await SendAsync(updateCommand);
            // Now validate in the database that our item only has a single child
            var allNonCapitalItems = await CallContextMethod<IList<NonCapitalItem>>("GetAllNonCapitalItems");
            var newItem = allNonCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.ExpenseItems.Should().HaveCount(1);
        }

        [Test]
        public async Task ShouldRequireValidDateStringOnUpdate()
        {
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            // Now update the item
            var updateCommand = new UpdateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    Units = 5,
                    DatePurchased = "077-11-1987"
                },
                NonCapitalItemId = item.Id
            };
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            updateCommand.NonCapitalItem.DatePurchased = "89chfkjhae";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            updateCommand.NonCapitalItem.DatePurchased = "07-11-2021";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireUniqueNameDateComboOnUpdate()
        {
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            // Create a second item
            createCommand.NonCapitalItem = new NonCapitalItemRequestDto
            {
                    Name = "MyItem2",
                    UnitCost = 0.25,
                    Units = 25,
                    DatePurchased = "07-11-1921", 
            };
            item = await SendAsync(createCommand);
            // Now update the item and attempt to use the same name/date as the first and second (second is fine since the id matches)
            var updateCommand = new UpdateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 4.45,
                    Units = 5,
                    DatePurchased = "07-11-2021"
                },
                NonCapitalItemId = item.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.NonCapitalItem.Name = "MyItem2";
            updateCommand.NonCapitalItem.DatePurchased = "07-11-1921";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
            updateCommand.NonCapitalItem.Name = "UpdatedItem";
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
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now update the item
            var updateCommand = new UpdateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    Units = 5,
                    DatePurchased = "07-11-1987",
                    ExpenseItemIds = new List<int>() { -1 }
                },
                NonCapitalItemId = item.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.NonCapitalItem.ExpenseItemIds = new List<int>() { expenseItem.Id };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>(); 
        }

        [Test]
        public async Task ShouldRequireValidNonCapitalItemIdOnUpdate()
        {
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            item.Name.Should().Be("MyItem");
            // Now update the item
            var updateCommand = new UpdateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "UpdatedItem!",
                    UnitCost = 4.45,
                    Units = 5,
                    DatePurchased = "07-11-1987",
                },
                NonCapitalItemId = -1
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.NonCapitalItemId = item.Id;
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        // Delete Tests
        [Test]
        public async Task ShouldRequireValidNonCapitalItemId()
        {
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            var deleteCommand = new DeleteNonCapitalItemCommand
            {
                NonCapitalItemId = -1
            };
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().Throw<ValidationException>();
            deleteCommand.NonCapitalItemId = item.Id;
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().NotThrow<ValidationException>();
        }

        [Test]
        public async Task ShouldDeleteNonCapitalItem()
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
            var createCommand = new CreateNonCapitalItemCommand
            {
                NonCapitalItem = new NonCapitalItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                }
            };
            var item = await SendAsync(createCommand);
            // Validate items exist in the database
            var allNonCapitalItems = await CallContextMethod<IList<NonCapitalItem>>("GetAllNonCapitalItems");
            var newItem = allNonCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.Should().NotBeNull();
            newItem.ExpenseItems.Should().HaveCount(1);
            // Now delete the non capital item and ensure children are deleted as well
            var deleteCommand = new DeleteNonCapitalItemCommand
            {
                NonCapitalItemId = item.Id
            };
            item = await SendAsync(deleteCommand);
            item.Name.Should().Be("MyItem");
            // Now validate in the database
            allNonCapitalItems = await CallContextMethod<IList<NonCapitalItem>>("GetAllNonCapitalItems");
            newItem = allNonCapitalItems.SingleOrDefault(item => item.Name.Equals("MyItem"));
            newItem.Should().BeNull();
            var allExpenseItems = await CallContextMethod<IList<ExpenseItem>>("GetAllExpenseItems");
            // Children expense items should also be deleted (since parent item has been deleted)
            var oldExpenseItem = allExpenseItems.SingleOrDefault(ei => ei.Name.Equals("Postage") && ei.DatePurchased.ToShortDateString().Equals("10/11/2021"));
            oldExpenseItem.Should().BeNull();
        }
    }
}