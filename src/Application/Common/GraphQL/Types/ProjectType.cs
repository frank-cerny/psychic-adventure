using GraphQL.Types;
using bike_selling_app.Domain.Entities;
using bike_selling_app.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    public class ProjectType : ObjectGraphType<Project>
    {
        public ProjectType(IServiceScopeFactory scopeFactory) 
        {
            Field(p => p.Id);
            Field(p => p.DateStarted, nullable: true);
            Field(p => p.DateEnded, nullable: true);
            Field(p => p.NetValue);
            // The name of the field should match the name in the entity (not case sensitive though)
            FieldAsync<ListGraphType<BikeType>, IList<Bike>>(
                name: "bikes",
                resolve: async context =>
                {
                    var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var allBikes = await dbContext.GetAllBikes();
                    return allBikes.Where(b => b.ProjectId == context.Source.Id).ToList();
                }
            );
            // TODO - Add capital item, non capital item, and revenue items when they are ready
        }
    }
}