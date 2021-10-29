using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required()]
        public string FirstName { get; set; }

        [Required()]
        public string LastName { get; set; }

        [Required()]
        public string UserName { get; set; }

        [Required()]
        public string Password { get; set; }

        public ICollection<Meetup> OrganizedMeetups { get; set; }

        [NotMapped]
        public string FullName { get { return this.FirstName + " " + this.LastName; } }
    }
}
