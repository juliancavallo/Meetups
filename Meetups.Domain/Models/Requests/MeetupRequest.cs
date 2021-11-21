using System;
using System.Collections.Generic;
using System.Text;

namespace Meetups.Domain.Models.Requests
{
    public class MeetupRequest
    {
        public int Id { get; set; }
        public DateTime MeetupDate { get; set; }
        public string Description { get; set; }
        public List<int> Attendees { get; set; }
    }
}
