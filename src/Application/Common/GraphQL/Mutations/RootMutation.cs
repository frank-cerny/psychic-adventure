using GraphQL.Types;
using GraphQL;
using bike_selling_app.Application.Common.GraphQL.Types;
using bike_selling_app.Application.Bikes.Commands;
using bike_selling_app.Domain.Entities;
using MediatR;

namespace bike_selling_app.Application.Common.GraphQL.Mutations
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation(ISender mediator)
        {
            FieldAsync<BikeType>(
            "addBike",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<BikeInputType>> { Name = "bike"}
            ),
            resolve: async context =>
            {
                var bike = context.GetArgument<BikeRequestDto>("bike");
                // Call the CreateBikeCommand to handle the rest :)
                var command = new CreateBikeCommand {
                    bike = bike
                };
                // This call returns a bike object, which is automatically converted to BikeType by this field
                return await mediator.Send<Bike>(command);
            });
        }
    }
}