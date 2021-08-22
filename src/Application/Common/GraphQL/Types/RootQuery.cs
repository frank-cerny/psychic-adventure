using GraphQL.Types;

namespace bike_selling_app.Application.GraphQL.Types
{
    public class RootQuery : ObjectGraphType
    {
        // Reference: https://graphql-dotnet.github.io/docs/getting-started/query-organization/
        public RootQuery()
        {
            // Field<GameStoreQuery>(
            //     name: "gamestore",
            //     resolve: context => new { }
            // );
            // Field<GameQuery>(
            //     name: "game",
            //     resolve: context => new { }
            // );
        }
    }
}