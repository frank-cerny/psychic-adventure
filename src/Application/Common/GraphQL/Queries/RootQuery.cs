using GraphQL.Types;
using bike_selling_app.Application.Common.GraphQL.Types;

namespace bike_selling_app.Application.Common.GraphQL.Queries
{
    public class RootQuery : ObjectGraphType
    {
        // Reference: https://graphql-dotnet.github.io/docs/getting-started/query-organization/
        public RootQuery()
        {
            // This allows us to nest all queries under a single root query
            Field<BikeQuery> (
                name: "bikes",
                resolve: context => new { }
            );
        }
    }
}