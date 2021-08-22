using GraphQL.Types;
using bike_selling_app.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using bike_selling_app.Application.Common.GraphQL.Types;
using System.Collections.Generic;
using bike_selling_app.Application.Common.Interfaces;

namespace bike_selling_app.Application.Common.GraphQL.Queries
{
    public class BikeQuery : ObjectGraphType
    {
        public BikeQuery(IServiceScopeFactory scopeFactory)
        {
            // This says to map between a list of Bike entities, to an output list of BikeType (to be passed to the user)
            FieldAsync<ListGraphType<BikeType>, IList<Bike>> (
                name: "all",
                resolve: async context => 
                {
                    var dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    return await dbContext.GetAllBikes();
                }
            );
        }
    }
}