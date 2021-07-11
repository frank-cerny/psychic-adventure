using bike_selling_app.Application.Common.Interfaces;
using System;

namespace bike_selling_app.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
