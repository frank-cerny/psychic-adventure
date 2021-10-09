using GraphQL.Types;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    // This maps to BikeRequestDto.cs
    public class ExpenseItemInputType : InputObjectGraphType
    {
        public ExpenseItemInputType()
        {
            Field<NonNullGraphType<FloatGraphType>>("unitCost");
            Field<NonNullGraphType<IntGraphType>>("units");
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<StringGraphType>("description");
            Field<StringGraphType>("datePurchased");
            Field<NonNullGraphType<IntGraphType>>("parentItemId");
        }
    }
}