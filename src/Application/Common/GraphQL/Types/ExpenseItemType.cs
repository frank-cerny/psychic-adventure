using GraphQL.Types;
using bike_selling_app.Domain.Entities;

namespace bike_selling_app.Application.Common.GraphQL.Types
{
    // This maps to BikeRequestDto.cs
    public class ExpenseItemType : ObjectGraphType<ExpenseItem>
    {
        public ExpenseItemType()
        {
            Field(e => e.Id);
            Field(e => e.Name, nullable: false);
            Field(e => e.UnitCost, nullable: false);
            Field(e => e.Units, nullable: false);
            Field(e => e.Description);
            Field(e => e.DatePurchased);
            Field(e => e.ParentItemId);
        }
    }
}