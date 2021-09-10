using AutoMapper;
using bike_selling_app.Application.Common.Mappings;
using bike_selling_app.Domain.Entities;
using System;
using System.Collections.Generic;

namespace bike_selling_app.Application.Projects.Commands
{
    public class ProjectRequestDto : IMapFrom<Project>
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public IList<int> BikeIds { get; set; } = new List<int>();
        public IList<int> CapitalItemIds { get; set; } = new List<int>();
        public IList<int> NonCapitalItemIds { get; set; } = new List<int>();
        public IList<int> RevenueItemIds { get; set; } = new List<int>();
        public string DateStarted { get; set; }
        public string DateEnded { get; set; }

    }
}