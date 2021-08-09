using GraphQL.Types;
using bike_selling_app.Domain.Entities;

namespace bike_selling_app.Domain.GraphQL.Types
{
    public sealed class BikeObject : ObjectGraphType<Bike>
    {
        public BikeObject()
        {
            Name = nameof(Bike);
            Description = "A single bike";

            Field(m => m.Id).Description("Identifier of the bike in the database");
            // TODO - Add other fields here
        }
    }
}