using GraphQL.Types;
using GraphQL.Utilities;
using System;
using Microsoft.Extensions.DependencyInjection;
using bike_selling_app.Application.Common.GraphQL.Queries;
using bike_selling_app.Application.Common.GraphQL.Mutations;

namespace bike_selling_app.Application.Common.GraphQL
{
    public class BikeAppSchema : Schema
    {
        public BikeAppSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<RootQuery>();
            Mutation = serviceProvider.GetRequiredService<RootMutation>();
        }
    }
}