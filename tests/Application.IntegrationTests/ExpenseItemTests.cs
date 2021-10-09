using bike_selling_app.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using bike_selling_app.Application.ExpenseItems.Commands;
using bike_selling_app.Application.Common.Exceptions;
using System.Linq;
using System.Globalization;

namespace bike_selling_app.Application.IntegrationTests.ExpenseItems
{
    using static Testing;
    public class BikeTests : TestBase
    {
        // Create Tests

        public async Task ShouldRequireUniqueNameDateComboOnCreate()
        {
            
        }

        public async Task ShouldRequireValidDateStringOnCreate()
        {

        }

        public async Task ShouldRequireValidParentItemIdOnCreate()
        {

        }

        public async Task ShouldCreateExpenseItem()
        {

        }

        // Update Tests

        public async Task ShouldRequireValidExpenseItemIdOnUpdate()
        {

        }

        public async Task ShouldRequireUniqueNameDateComboOnUpdate()
        {

        }

        public async Task ShouldRequireValidParentItemIdOnUpdate()
        {
            
        }

        public async Task ShouldUpdateExpenseItem()
        {

        }

        // Delete Tests

        public async Task ShouldRequireValidExpenseItemIdOnDelete()
        {

        }

        public async Task ShouldDeleteExpenseItem()
        {
            
        }
    }
}