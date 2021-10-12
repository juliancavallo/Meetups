using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Filters
{
    public class MeetupSearchFilter
    {
        public string Description { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}
