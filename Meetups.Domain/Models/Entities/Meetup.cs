using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Meetups.Domain.Models.Entities
{
    public class Meetup
    {
        public int Id { get; set; }

        public DateTime MeetupDate { get; set; }

        public decimal Temperature { get; set; }

        [ForeignKey("OrganizerId")]
        public User Organizer { get; set; }

        public int OrganizerId { get; set; }

        [Required()]
        public string Description { get; set; }

        [NotMapped]
        public ICollection<MeetupUser> Attendees { get; set; }

        [NotMapped]
        public int BeerBoxQuantity
        {
            get
            {
                if (Attendees != null)
                {
                    decimal totalBeers;

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
