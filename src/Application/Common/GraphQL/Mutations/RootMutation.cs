using GraphQL.Types;
using GraphQL;
using bike_selling_app.Application.Common.GraphQL.Types;
using bike_selling_app.Application.Bikes.Commands;
using bike_selling_app.Application.Projects.Commands;
using bike_selling_app.Application.ExpenseItems.Commands;
using bike_selling_app.Application.NonCapitalItems.Commands;
using bike_selling_app.Domain.Entities;
using MediatR;

namespace bike_selling_app.Application.Common.GraphQL.Mutations
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation(ISender mediator)
        {

            // Bikes
            FieldAsync<BikeType>(
            "addBike",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<BikeInputType>> { Name = "bike" }
            ),
            resolve: async context =>
            {
                var bike = context.GetArgument<BikeRequestDto>("bike");
                // Call the CreateBikeCommand to handle the rest :)
                var command = new CreateBikeCommand
                {
                    bike = bike
                };
                // This call returns a bike object, which is automatically converted to BikeType by this field
                return await mediator.Send<Bike>(command);
            });

            FieldAsync<BikeType>(
            "removeBike",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
            ),
            resolve: async context =>
            {
                var command = new DeleteBikeCommand
                {
                    bikeId = context.GetArgument<int>("id")
                };
                return await mediator.Send<Bike>(command);
            });

            FieldAsync<BikeType>(
            "updateBike",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<BikeInputType>> { Name = "bike" },
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
            ),
            resolve: async context =>
            {
                var bike = context.GetArgument<BikeRequestDto>("bike");
                var bikeId = context.GetArgument<int>("id");
                var command = new UpdateBikeCommand
                {
                    bike = bike,
                    bikeId = bikeId
                };
                // This call returns a bike object, which is automatically converted to BikeType by this field
                return await mediator.Send<Bike>(command);
            });

            // Projects
            FieldAsync<ProjectType>(
                "addProject",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ProjectInputType>> { Name = "project" }
                ),
                resolve: async context =>
                {
                    var project = context.GetArgument<ProjectRequestDto>("project");
                    var command = new CreateProjectCommand
                    {
                        project = project
                    };
                    return await mediator.Send(command);
                });

            FieldAsync<ProjectType>(
                "removeProject",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var projectId = context.GetArgument<int>("id");
                    var command = new DeleteProjectCommand
                    {
                        projectId = projectId
                    };
                    return await mediator.Send(command);
                });

            FieldAsync<ProjectType>(
                "updateProject",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ProjectInputType>> { Name = "project"},
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var project = context.GetArgument<ProjectRequestDto>("project");
                    var projectId = context.GetArgument<int>("id");
                    var updateCommand = new UpdateProjectCommand
                    {
                        project = project,
                        projectId = projectId
                    };
                    return await mediator.Send(updateCommand);
                });

            // Expense Items
            FieldAsync<ExpenseItemType>(
                "addExpenseItem",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ExpenseItemInputType>> { Name = "expenseItem"}
                ),
                resolve: async context =>
                {
                    var expenseItem = context.GetArgument<ExpenseItemRequestDto>("expenseItem");
                    var createCommand = new CreateExpenseItemCommand
                    {
                        ExpenseItem = expenseItem
                    };
                    return await mediator.Send(createCommand);
                });

            FieldAsync<ExpenseItemType>(
                "updateExpenseItem",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ExpenseItemInputType>> { Name = "expenseItem"},
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id"}
                ),
                resolve: async context =>
                {
                    var expenseItem = context.GetArgument<ExpenseItemRequestDto>("expenseItem");
                    var expenseItemId = context.GetArgument<int>("id");
                    var updateCommand = new UpdateExpenseItemCommand
                    {
                        ExpenseItem = expenseItem,
                        ExpenseItemId = expenseItemId
                    };
                    return await mediator.Send(updateCommand);
                });

            FieldAsync<ExpenseItemType>(
                "removeExpenseItem",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id"}
                ),
                resolve: async context =>
                {
                    var expenseItemId = context.GetArgument<int>("id");
                    var deleteCommand = new DeleteExpenseItemCommand
                    {
                        ExpenseItemId = expenseItemId
                    };
                    return await mediator.Send(deleteCommand);
                });

            // NonCapital Items
            FieldAsync<NonCapitalItemType>(
                "addNonCapitalItem",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<NonCapitalItemInputType>> { Name = "nonCapitalItem"}
                ),
                resolve: async context =>
                {
                    var nonCapitalItem = context.GetArgument<NonCapitalItemRequestDto>("nonCapitalItem");
                    var createCommand = new CreateNonCapitalItemCommand
                    {
                        NonCapitalItem = nonCapitalItem
                    };
                    return await mediator.Send(createCommand);
                });

            FieldAsync<NonCapitalItemType>(
                "updateNonCapitalItem",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<NonCapitalItemInputType>> { Name = "nonCapitalItem"},
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id"}
                ),
                resolve: async context =>
                {
                    var nonCapitalItem = context.GetArgument<NonCapitalItemRequestDto>("nonCapitalItem");
                    var nonCapitalItemId = context.GetArgument<int>("id");
                    var updateCommand = new UpdateNonCapitalItemCommand
                    {
                        NonCapitalItem = nonCapitalItem,
                        NonCapitalItemId = nonCapitalItemId
                    };
                    return await mediator.Send(updateCommand);
                });

            FieldAsync<NonCapitalItemType>(
                "deleteNonCapitalItem",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id"}
                ),
                resolve: async context =>
                {
                    var nonCapitalItemId = context.GetArgument<int>("id");
                    var deleteCommand = new DeleteNonCapitalItemCommand
                    {
                        NonCapitalItemId = nonCapitalItemId
                    };
                    return await mediator.Send(deleteCommand);
                });
        }
    }
}