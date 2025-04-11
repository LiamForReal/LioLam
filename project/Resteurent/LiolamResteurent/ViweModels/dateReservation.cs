using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class dateReservation
    {
        public DateTime ReserveDate { get; set; }
        public List<Reservation> DayReservations { get; set; }
    }
}
