using System;
using AutoMapper;
using bike_selling_app.Domain.Entities;
using bike_selling_app.Application.Common.Mappings;
using System.Collections.Generic;

namespace bike_selling_app.Application.NonCapitalItems.Commands
{
    public class NonCapitalItemRequestDto : IMapFrom<NonCapitalItem>
    {
        public double UnitCost { get; set; }
        public int UnitsPurchased { get; set; }
        // If this value is not set, the default value is UnitsPurchased (to be set in the commands, not validated because of this)
        public int UnitsRemaining { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public int? ProjectId { get; set; }

        // Default to an emtpy list to make validation a bit easier
        public IList<int> ExpenseItemIds { get; set; } = new List<int>();

        // Date will be of the form "mmddyyyy" (validated in the validator)
        public string DatePurchased { get; set; }

        // This creates a custom mapping where 1 -> 1 mapping is not available
        public void Mapping(Profile profile)
        {
            profile.CreateMap<NonCapitalItemRequestDto, NonCapitalItem>()
                .ForMember(b => b.DatePurchased, opt => opt.MapFrom(dto => DateTime.Parse(dto.DatePurchased)));
        }
    }
}