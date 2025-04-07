using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Reservations : Models
    {
        public Customers Customer { get; set; }//To change may
        public DateTime ReserveDate { get; set; }
        public int AmountOfPeople { get; set; }
        public string ReserveHour { get; set; }

        public Reservations() { }
        public Reservations(DateTime reserveDate, int amountOfPeople)
        {
            Customer = null;
            ReserveDate = reserveDate;
            AmountOfPeople = amountOfPeople;
            ReserveHour = reserveDate.ToString("HH:mm");
        }
    }
}
