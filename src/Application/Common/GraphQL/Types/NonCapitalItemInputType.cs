using GraphQL.Types;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    public class NonCapitalInputItemType : InputObjectGraphType
    {
        // This maps directly to ProjectRequestDTO (So GraphQL can automatically map on request)
        public NonCapitalInputItemType()
        {
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<StringGraphType>("description");
            Field<StringGraphType>("datePurchased");
            Field<IntGraphType>("projectId");
            Field<NonNullGraphType<FloatGraphType>>("unitCost");
            Field<NonNullGraphType<IntGraphType>>("units");
            Field<ListGraphType<IntGraphType>>("expenseItemIds");
        }
    }
}