using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class ReservationCreator : IModelCreator<Reservations>
    {
        /// <summary>
        /// Creates a Reservation model from an IDataReader source.
        /// </summary>
        public Reservations CreateModel(IDataReader src)
        {
            Reservations reservation = new Reservations()
            {
                Id = Convert.ToString(src["ReserveId"]),
                CustomerId = "",
                ReserveDate = Convert.ToDateTime(src["ReserveDate"]),
                AmountOfPeople = Convert.ToInt32(src["AmountOfPeople"]),
                ReserveHour = Convert.ToString(src["ReserveHour"])
            };
            return reservation;
        }
    }
}
