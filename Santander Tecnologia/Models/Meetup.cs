using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Santander_Tecnologia.Models
{
    public class Meetup
    {
        public int Id { get; set; }
        public DateTime MeetupDate { get; set; }
        public int Attendees { get; set; }
        public decimal Temperature { get; set; }
        [NotMapped]
        public int BeerBoxQuantity 
        {
            get 
            {
                decimal totalBeers = 0;

                if (Temperature < 20)
                {
                    totalBeers = Convert.ToDecimal((0.75 * Attendees));
                } 
                else if(Temperature < 24)
                {
                    totalBeers = Attendees;
                }
                else
                {
                    totalBeers = 2 * Attendees;
                }

                return Convert.ToInt32(Math.Ceiling(totalBeers / 6));
            }
        }
    }
}
