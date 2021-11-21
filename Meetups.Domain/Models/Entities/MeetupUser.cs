using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Meetups.Domain.Models.Entities
{
    public class MeetupUser
    {
        public int Id { get; set; }
        [Required]
        public Meetup Meetup { get; set; }
        public int MeetupId { get; set; }

        [Required]
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
