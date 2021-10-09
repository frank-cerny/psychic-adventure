using System;
using AutoMapper;
using bike_selling_app.Domain.Entities;

namespace bike_selling_app.Application.ExpenseItems
{
    public class ExpenseItemDto
    {
        public double UnitCost { get; set; }
        public int Units { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentItemId { get; set; }

        // Date will be of the form "mmddyyyy" (validated in the validator)
        public string DatePurchased { get; set; }

        // This creates a custom mapping where 1 -> 1 mapping is not available
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExpenseItemDto, ExpenseItem>()
                .ForMember(b => b.DatePurchased, opt => opt.MapFrom(dto => DateTime.Parse(dto.DatePurchased)));
        }
    }
}