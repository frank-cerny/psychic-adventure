using GraphQL.Types;
using bike_selling_app.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using bike_selling_app.Application.Common.GraphQL.Types;
using System.Collections.Generic;
using bike_selling_app.Application.Common.Interfaces;
using GraphQL;

namespace bike_selling_app.Application.Common.GraphQL.Queries
{
    public class RootQuery : ObjectGraphType
    {
        // Reference: https://graphql-dotnet.github.io/docs/getting-started/query-organization/        
        public RootQuery(IServiceScopeFactory scopeFactory)
        {
            // This says to map between a list of Bike entities, to an output list of BikeType (to be passed to the user)
            FieldAsync<ListGraphType<BikeType>, IList<Bike>> (
                name: "bikes",
                resolve: async context => 
                {
                    var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    return await dbContext.GetAllBikes();
                }
            );
            FieldAsync<ListGraphType<ProjectType>, IList<Project>> (
                name: "projects",
                resolve: async context =>
                {
                    var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    return await dbContext.GetAllProjects();
                }
            );
        }
    }
}