using GraphQL.Types;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    public class CapitalItemInputType : InputObjectGraphType
    {
        public CapitalItemInputType()
        {
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<StringGraphType>("description");
            Field<StringGraphType>("datePurchased");
            Field<NonNullGraphType<FloatGraphType>>("cost");
            Field<ListGraphType<IntGraphType>>("expenseItemIds");
            Field<ListGraphType<IntGraphType>>("projectIds");
        }
    }
}