using System;
using GraphQL.Types;

namespace bike_selling_app.Domain.GraphQL
{
    public class BikeProjectSchema : Schema
    {
        public BikeProjectSchema(QueryObject query, MutationObject mutation, IServiceProvider sp) : base(sp)
        {
            Query = query;
            Mutation = mutation;
        }
    }
}