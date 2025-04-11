using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Reservation : Model
    {
        public string CustomerId { get; set; }//To change may
        public DateTime ReserveDate { get; set; }
        public int AmountOfPeople { get; set; }
        public string ReserveHour { get; set; }

        public Reservation(DateTime reserveDate, int amountOfPeople)
        {
            CustomerId = "";
            ReserveDate = reserveDate;
            AmountOfPeople = amountOfPeople;
            ReserveHour = reserveDate.ToString("HH:mm");
        }

        public Reservation()
        {
        }
    }
}
