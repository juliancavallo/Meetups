using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Meetup
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.Resource), Name = "MeetupDate")]
        public DateTime MeetupDate { get; set; }

        [Display(ResourceType = typeof(Resources.Resource), Name = "Temperature")]
        public decimal Temperature { get; set; }

        [ForeignKey("OrganizerId")]
        [Display(ResourceType = typeof(Resources.Resource), Name = "Organizer")]
        public User Organizer { get; set; }

        public int OrganizerId { get; set; }

        [Display(ResourceType = typeof(Resources.Resource), Name = "Description")]
        [Required()]
        public string Description { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Resource), Name = "Attendees")]
        public ICollection<MeetupUsers> Attendees { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof(Resources.Resource), Name = "BeerBoxQuantity")]
        public int BeerBoxQuantity
        {
            get
            {
                if (Attendees != null)
                {
                    decimal totalBeers = 0;

                    if (Temperature < 20)
                    {
                        totalBeers = Convert.ToDecimal((0.75 * Attendees.Count));
                    }
                    else if (Temperature <= 24)
                    {
                        totalBeers = Attendees.Count;
                    }
                    else
                    {
                        totalBeers = 2 * Attendees.Count;
                    }

                    return Convert.ToInt32(Math.Ceiling(totalBeers / 6));
                }
                else
                    return 0;
            }
        }
    }
}
