using GraphQL.Types;
using bike_selling_app.Domain.Entities;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    // This maps to BikeRequestDto.cs
    public class BikeInputType : InputObjectGraphType
    {
        public BikeInputType()
        {
            Field<NonNullGraphType<StringGraphType>>("serialNumber");
            Field<NonNullGraphType<StringGraphType>>("make");
            Field<NonNullGraphType<StringGraphType>>("model");
            Field<NonNullGraphType<StringGraphType>>("purchasedFrom");
            Field<FloatGraphType>("purchasePrice");
            Field<IntGraphType>("projectId");
            Field<NonNullGraphType<StringGraphType>>("datePurchased");
        }
    }
}