using System;
using System.Collections.Generic;
using System.Text;

namespace Meetups.Domain.Filters
{
    public class MeetupSearchFilter
    {
        public MeetupSearchFilter(string description = null, DateTime? dateFrom = null, DateTime? dateTo = null, int? id = null)
        {
            Description = description;
            DateFrom = dateFrom;
            DateTo = dateTo;
            Id = id;
        }

        public MeetupSearchFilter() { }

        public string Description { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public int? Id { get; set; }
    }
}
