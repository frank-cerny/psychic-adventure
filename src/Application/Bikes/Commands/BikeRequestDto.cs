using bike_selling_app.Application.Common.Mappings;
using bike_selling_app.Domain.Entities;
using AutoMapper;
using System;

namespace bike_selling_app.Application.Bikes.Commands
{
    public class BikeRequestDto : IMapFrom<Bike>
    {
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public double PurchasePrice { get; set; }
        public string PurchasedFrom { get; set; }
        public int? ProjectId { get; set; }

        // Date will be of the form "mmddyyyy" (validated in the validator)
        public string DatePurchased { get; set; }

        // This creates a custom mapping where 1 -> 1 mapping is not available
        public void Mapping(Profile profile)
        {
            profile.CreateMap<BikeRequestDto, Bike>()
                .ForMember(b => b.DatePurchased, opt => opt.MapFrom(dto => DateTime.Parse(dto.DatePurchased)));
        }
    }
}