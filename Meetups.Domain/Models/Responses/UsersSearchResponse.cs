using Meetups.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Meetups.Domain.Models.Responses
{
    public class UsersSearchResponse
    {
        public List<User> Users { get; set; }
    }
}
