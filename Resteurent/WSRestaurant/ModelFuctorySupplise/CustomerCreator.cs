using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class CustomerCreator : IModelCreator<Customers>
    {
        /// <summary>
        /// Creates a Customer model from an IDataReader source.
        /// </summary>
        public Customers CreateModel(IDataReader src)
        {
            Customers customer = new Customers()
            {
                CustomerId = Convert.ToString(src["CustomerId"]),
                CustomerUserName = Convert.ToString(src["CustomerUserName"]),
                CustomerPassword = Convert.ToString(src["CustomerPassword"]),
                CustomerImage = Convert.ToString(src["CustomerImage"]),
                CustomerHouse = Convert.ToInt32(src["CustomerHouse"]),
                CustomerPhone = Convert.ToString(src["CustomerPhone"]),
                CustomerMail = Convert.ToString(src["CustomerMail"]),
                cityName = "",
                streetName = "",
                CurrentReservation = null,
                orders = null
            };
            return customer;
        }
    }
}
