using GraphQL.Types;
using bike_selling_app.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using bike_selling_app.Application.Common.Interfaces;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    public class BikeType : ObjectGraphType<Bike>
    {
        public BikeType(IServiceScopeFactory scopeFactory)
        {
            Field(b => b.Id);
            Field(b => b.SerialNumber);
            Field(b => b.Make);
            Field(b => b.Model);
            Field(b => b.PurchasePrice);
            Field(b => b.PurchasedFrom);
            // Reference: https://bartwullems.blogspot.com/2018/11/graphqldotnetnullable-types-nullable.html
            Field(b => b.ProjectId, nullable: true);
            Field(b => b.DatePurchased);
            FieldAsync<ProjectType, Project>(
                name: "project",
                resolve: async context =>
                {
                    var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    // Only return the project if the bike is assigned one
                    if (context.Source.ProjectId.HasValue)
                    {
                        return await dbContext.GetProjectById(context.Source.ProjectId.Value);
                    }
                    return null;
                }
            );
        }
    }
}