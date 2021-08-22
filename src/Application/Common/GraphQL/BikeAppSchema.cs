using GraphQL.Types;
using GraphQL.Utilities;
using System;
using Microsoft.Extensions.DependencyInjection;
using bike_selling_app.Application.GraphQL.Types;

namespace bike_selling_app.Application.GraphQL
{
    public class BikeAppSchema : Schema
    {
        public BikeAppSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<RootQuery>();
            // Mutation = serviceProvider.GetRequiredService<RootMutation>();
        }
    }
}