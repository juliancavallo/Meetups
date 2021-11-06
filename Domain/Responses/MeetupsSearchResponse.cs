using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Responses
{
    public class MeetupsSearchResponse
    {
        public List<Meetup> Meetups { get; set; }
    }
}
