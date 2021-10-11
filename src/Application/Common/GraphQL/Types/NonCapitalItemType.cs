using GraphQL.Types;
using bike_selling_app.Domain.Entities;
using bike_selling_app.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    public class NonCapitalItemType : ObjectGraphType<NonCapitalItem>
    {
        public NonCapitalItemType(IServiceScopeFactory scopeFactory)
        {
            Field(e => e.Id);
            Field(e => e.Name, nullable: false);
            Field(e => e.UnitCost, nullable: false);
            Field(e => e.Units, nullable: false);
            Field(e => e.Description);
            Field(e => e.DatePurchased);
            Field(e => e.ProjectId);
            FieldAsync<ListGraphType<ExpenseItemType>, IList<ExpenseItem>>(
                name: "expenseItems",
                resolve: async context =>
                {
                    var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var allExpenseItems = await dbContext.GetAllExpenseItems();
                    return allExpenseItems.Where(ei => ei.NonCapitalItemId == context.Source.Id).ToList();
                }
            );
        }
    }
}