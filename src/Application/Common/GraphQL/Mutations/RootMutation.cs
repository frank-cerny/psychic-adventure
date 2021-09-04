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
            FieldAsync<BikeType>(
            "removeBike",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id"}
            ),
            resolve: async context =>
            {
                var command = new DeleteBikeCommand {
                    bikeId = context.GetArgument<int>("id")
                };
                return await mediator.Send<Bike>(command);
            });
            FieldAsync<BikeType>(
            "updateBike",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<BikeInputType>> { Name = "bike"},
                new QueryArgument<NonNullGraphType<IntGraphType>>  { Name = "id"}
            ),
            resolve: async context =>
            {
                var bike = context.GetArgument<BikeRequestDto>("bike");
                var bikeId = context.GetArgument<int>("id");
                var command = new UpdateBikeCommand {
                    bike = bike,
                    bikeId = bikeId
                };
                // This call returns a bike object, which is automatically converted to BikeType by this field
                return await mediator.Send<Bike>(command);
            });
        }
    }
}