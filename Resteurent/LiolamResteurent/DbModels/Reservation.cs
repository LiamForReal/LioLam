using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Reservation : IModel
    {
        public string CustomerId { get; set; }//To change may
        public DateTime ReserveDate { get; set; }
        public int AmountOfPeople { get; set; }
        public string ReserveHour { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Reservation reservation)
                return this.Id == reservation.Id && reservation.CustomerId == this.CustomerId && 
                this.ReserveDate == reservation.ReserveDate && this.AmountOfPeople == reservation.AmountOfPeople
                && this.ReserveHour == reservation.ReserveHour;
            return false;
        }
    }
}
