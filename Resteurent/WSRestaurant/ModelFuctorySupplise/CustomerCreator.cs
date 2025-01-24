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
                Id = Convert.ToString(src["CustomerId"]),
                CustomerUserName = Convert.ToString(src["CustomerUserName"]),
                CustomerPassword = Convert.ToString(src["CustomerPassword"]),
                CustomerImage = "http://localhost:5125/Images/Customers/" + Convert.ToString(src["CustomerImage"]),
                CustomerHouse = Convert.ToInt32(src["CustomerHouse"]),
                CustomerPhone = Convert.ToString(src["CustomerPhone"]),
                CustomerMail = Convert.ToString(src["CustomerMail"]),
                city = null,
                street = null,
                //CurrentReservation = null,
                //orders = null
            };
            return customer;
        }
    }
}
