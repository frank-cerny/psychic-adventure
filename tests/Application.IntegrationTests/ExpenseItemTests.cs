using bike_selling_app.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using bike_selling_app.Application.ExpenseItems.Commands;
using bike_selling_app.Application.Common.Exceptions;
using System.Linq;
using System.Globalization;
using System;

namespace bike_selling_app.Application.IntegrationTests.ExpenseItems
{
    using static Testing;
    public class BikeTests : TestBase
    {
        // Create Tests

        public async Task ShouldRequireUniqueNameDateComboOnCreate()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ParentItemId = parentId
                }
            };
            var item = await SendAsync(createCommand);
            createCommand.ExpenseItem = new ExpenseItemRequestDto
            {
                Name = "MyItem",
                UnitCost = 5.67,
                Units = 1,
                DatePurchased = "07-11-2021"
            };
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.ExpenseItem.Name = "MyItem2";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        public async Task ShouldRequireValidDateStringOnCreate()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "077-11-2021",
                    ParentItemId = parentId
                }
            };
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.ExpenseItem.DatePurchased = "89chfkjhae";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.ExpenseItem.DatePurchased = "07-11-2021";
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        public async Task ShouldRequireValidParentItemIdOnCreate()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "077-11-2021",
                    ParentItemId = -1
                }
            };
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().Throw<ValidationException>();
            createCommand.ExpenseItem.ParentItemId = parentId;
            FluentActions.Invoking(() => SendAsync(createCommand)).Should().NotThrow<ValidationException>();
        }

        public async Task ShouldCreateExpenseItem()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ParentItemId = parentId
                }
            };
            var item = await SendAsync(createCommand);
            // Now validate the updated item in the database matches
            var expenseItems = await CallContextMethod<List<ExpenseItem>>("GetAllExpenseItems", new object());
            var newItem = expenseItems.SingleOrDefault(e => e.Name.Equals("MyItem"));
            newItem.Should().NotBeNull();
            newItem.UnitCost.Should().Be(5.68);
            newItem.Units.Should().Be(5);
            CultureInfo culture = new CultureInfo("en-US");
            newItem.DatePurchased.ToShortDateString().Should().Be("7/15/2021");
            newItem.ParentItemId.Should().Be(parentId);
        }

        // Update Tests

        public async Task ShouldRequireValidExpenseItemIdOnUpdate()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ParentItemId = parentId
                }
            };
            var item = await SendAsync(createCommand);
            var updateCommand = new UpdateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItemNewName",
                    UnitCost = 5.68,
                    Units = 5,
                    DatePurchased = "07-15-2021",
                    ParentItemId = parentId
                },
                ExpenseItemId = -1
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.ExpenseItemId = item.Id;
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        public async Task ShouldRequireUniqueNameDateComboOnUpdate()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ParentItemId = parentId
                }
            };
            var item = await SendAsync(createCommand);
            createCommand.ExpenseItem = new ExpenseItemRequestDto
            {
                Name = "MyItem2",
                UnitCost = 5.68,
                Units = 1,
                DatePurchased = "07-11-2021",
                ParentItemId = parentId
            };
            var item2 = await SendAsync(createCommand);
            // Now attempt to update item 2 with the same name as item2!
            var updateCommand = new UpdateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.68,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ParentItemId = parentId
                },
                ExpenseItemId = item2.Id
            };
            // Date/Name should be unique across all items
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.ExpenseItem.DatePurchased = "07-12-2021";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
            updateCommand.ExpenseItem.DatePurchased = "07-11-2021";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            // Using the same name is obviously allowed!
            updateCommand.ExpenseItem.Name = "MyItem2";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
            updateCommand.ExpenseItem.Name = "NewNameForItem2";
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        public async Task ShouldRequireValidParentItemIdOnUpdate()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ParentItemId = parentId
                }
            };
            var item = await SendAsync(createCommand);
            // Send an invalid parent id the first time
            var updateCommand = new UpdateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItemNewName",
                    UnitCost = 5.68,
                    Units = 5,
                    DatePurchased = "07-15-2021",
                    ParentItemId = -1
                },
                ExpenseItemId = item.Id
            };
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().Throw<ValidationException>();
            updateCommand.ExpenseItem.ParentItemId = parentId;
            // Parent id is valid and should work now
            FluentActions.Invoking(() => SendAsync(updateCommand)).Should().NotThrow<ValidationException>();
        }

        public async Task ShouldUpdateExpenseItem()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ParentItemId = parentId
                }
            };
            var item = await SendAsync(createCommand);
            var updateCommand = new UpdateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItemNewName",
                    UnitCost = 5.68,
                    Units = 5,
                    DatePurchased = "07-15-2021",
                    ParentItemId = parentId
                },
                ExpenseItemId = item.Id
            };
            var updatedItem = await SendAsync(updateCommand);
            // Now validate the updated item in the database matches
            var expenseItems = await CallContextMethod<List<ExpenseItem>>("GetAllExpenseItems", new object());
            var newItem = expenseItems.SingleOrDefault(e => e.Name.Equals("MyItemNewName"));
            newItem.Should().NotBeNull();
            newItem.UnitCost.Should().Be(5.68);
            newItem.Units.Should().Be(5);
            CultureInfo culture = new CultureInfo("en-US");
            newItem.DatePurchased.ToShortDateString().Should().Be("7/15/2021");
            newItem.ParentItemId.Should().Be(parentId);
        }

        // Delete Tests

        public async Task ShouldRequireValidExpenseItemIdOnDelete()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ParentItemId = parentId
                }
            };
            var item = await SendAsync(createCommand);
            var deleteCommand = new DeleteExpenseItemCommand
            {
                ExpenseItemId = -1
            };
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().Throw<ValidationException>();
            deleteCommand.ExpenseItemId = item.Id;
            FluentActions.Invoking(() => SendAsync(deleteCommand)).Should().NotThrow<ValidationException>();
        }

        public async Task ShouldDeleteExpenseItem()
        {
            var parentId = await GetMockParentId();
            var createCommand = new CreateExpenseItemCommand
            {
                ExpenseItem = new ExpenseItemRequestDto
                {
                    Name = "MyItem",
                    UnitCost = 5.67,
                    Units = 1,
                    DatePurchased = "07-11-2021",
                    ParentItemId = parentId
                }
            };
            var item = await SendAsync(createCommand);
            var deleteCommand = new DeleteExpenseItemCommand
            {
                ExpenseItemId = item.Id
            };
            await SendAsync(deleteCommand);
            var expenseItems = await CallContextMethod<List<ExpenseItem>>("GetAllExpenseItems", new object());
            var newItem = expenseItems.SingleOrDefault(e => e.Name.Equals("MyItemNewName"));
            newItem.Should().BeNull();
        }

        // Add a random item to the database so that there is a valid id to be checked in the validator
        public async Task<int> GetMockParentId()
        {
            // Only create a new item if it does not already exist
            var capItems = await CallContextMethod<List<CapitalItem>>("GetAllCapitalItems", null);
            var newItem = capItems.SingleOrDefault(ci => ci.Name.Equals("TestItem"));
            if (newItem == null)
            {
                var capItem = new CapitalItem
                {
                    Name = "TestItem",
                    UsageCount = 1,
                    Cost = 25.67,
                    DatePurchased = DateTime.Today
                };
                await AddAsync<CapitalItem>(capItem);
                capItems = await CallContextMethod<List<CapitalItem>>("GetAllCapitalItems", null);
                newItem = capItems.SingleOrDefault(ci => ci.Name.Equals("TestItem"));
            }
            return newItem.Id;
        }
    }
}