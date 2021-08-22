using GraphQL.Types;
using bike_selling_app.Domain.Entities;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    public class BikeType : ObjectGraphType<Bike>
    {
        public BikeType()
        {
            Field(b => b.Id);
            Field(b => b.SerialNumber);
            Field(b => b.Make);
            Field(b => b.Model);
            Field(b => b.PurchasePrice);
            Field(b => b.PurchasedFrom);
            // Reference: https://bartwullems.blogspot.com/2018/11/graphqldotnetnullable-types-nullable.html
            Field(b => b.ProjectId, nullable: true);
            Field(b => b.DatePurchased);
        }
    }
}