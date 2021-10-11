using GraphQL.Types;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    public class NonCapitalItemInputType : InputObjectGraphType
    {
        // This maps directly to ProjectRequestDTO (So GraphQL can automatically map on request)
        public NonCapitalItemInputType()
        {
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<StringGraphType>("description");
            Field<StringGraphType>("datePurchased");
            Field<IntGraphType>("projectId");
            Field<NonNullGraphType<FloatGraphType>>("unitCost");
            Field<NonNullGraphType<IntGraphType>>("unitsPurchased");
            Field<NonNullGraphType<IntGraphType>>("unitsRemaining");
            Field<ListGraphType<IntGraphType>>("expenseItemIds");
        }
    }
}