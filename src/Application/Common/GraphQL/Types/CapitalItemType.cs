using GraphQL.Types;
using bike_selling_app.Domain.Entities;
using bike_selling_app.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    public class CapitalItemType : ObjectGraphType<CapitalItem>
    {
        public CapitalItemType(IServiceScopeFactory scopeFactory)
        {
            Field(e => e.Id);
            Field(e => e.Name, nullable: false);
            Field(e => e.Cost, nullable: false);
            Field(e => e.UsageCount, nullable: false);
            Field(e => e.Description);
            Field(e => e.DatePurchased);
            FieldAsync<ListGraphType<ExpenseItemType>, IList<ExpenseItem>>(
                name: "expenseItems",
                resolve: async context =>
                {
                    var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var allExpenseItems = await dbContext.GetAllExpenseItems();
                    return allExpenseItems.Where(ei => ei.CapitalItemId == context.Source.Id).ToList();
                }
            );
            FieldAsync<ListGraphType<ProjectType>, IList<Project>>(
                name: "projects",
                resolve: async context => {
                    var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var allProjects = await dbContext.GetAllProjects();
                    return allProjects.Where(p => p.CapitalItems.Select(ci => ci.Id).ToList().Contains(context.Source.Id)).ToList();
                }
            );
        }
    }
}