using Meetups.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meetups.Domain.Models.Responses
{
    public class MeetupsSearchResponse
    {
        public List<Meetup> Meetups { get; set; }
    }
}
