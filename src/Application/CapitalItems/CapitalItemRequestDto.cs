using System;
using AutoMapper;
using bike_selling_app.Domain.Entities;
using bike_selling_app.Application.Common.Mappings;
using System.Collections.Generic;

namespace bike_selling_app.Application.CapitalItems.Commands
{
    public class CapitalItemRequestDto : IMapFrom<CapitalItem>
    {
        public double Cost { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";
        public List<int> ProjectIds { get; set; } = new List<int>();
        // Default to an emtpy list to make validation a bit easier
        public IList<int> ExpenseItemIds { get; set; } = new List<int>();

        // Date will be of the form "mmddyyyy" (validated in the validator)
        public string DatePurchased { get; set; }

        // This creates a custom mapping where 1 -> 1 mapping is not available
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CapitalItemRequestDto, CapitalItem>()
                .ForMember(b => b.DatePurchased, opt => opt.MapFrom(dto => DateTime.Parse(dto.DatePurchased)));
        }
    }
}