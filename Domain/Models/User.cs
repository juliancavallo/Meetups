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
        [Display(ResourceType = typeof(Resources.Resource), Name = "Name")]
        public string Name { get; set; }
        [Required()]
        [Display(ResourceType = typeof(Resources.Resource), Name = "LastName")]
        public string LastName { get; set; }
        [Required()]
        [Display(ResourceType = typeof(Resources.Resource), Name = "UserName")]
        public string UserName { get; set; }
        [Required()]
        [Display(ResourceType = typeof(Resources.Resource), Name = "Password")]
        public string Password { get; set; }
        public ICollection<Meetup> OrganizedMeetups { get; set; }
        [NotMapped]
        [Display(ResourceType = typeof(Resources.Resource), Name = "FullName")]
        public string FullName { get { return this.Name + " " + this.LastName; } }
    }
}
