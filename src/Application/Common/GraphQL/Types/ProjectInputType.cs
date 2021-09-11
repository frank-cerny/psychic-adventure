using GraphQL.Types;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    public class ProjectInputType : InputObjectGraphType
    {
        public ProjectInputType()
        {
            Field<StringGraphType>("dateStarted");
            Field<StringGraphType>("dateEnded");
            Field<ListGraphType<IntGraphType>>("bikeIds");
            Field<StringGraphType>("description");
            Field<NonNullGraphType<StringGraphType>>("title");
            // TODO - Uncomment these as they become available
            // Field<ListGraphType<IntGraphType>>("capitalItemIds");
            // Field<ListGraphType<IntGraphType>>("nonCapitalItemIds");
            // Field<ListGraphType<IntGraphType>>("revenueItemIds");
        }
    }
}