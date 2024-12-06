using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class OrdersCreator : IModelCreator<Orders>
    {
        /// <summary>
        /// Creates a Order model from an IDataReader source.
        /// </summary>
        public Orders CreateModel(IDataReader src)
        {
            Orders order = new Orders()
            {
                OrderId = Convert.ToInt32(src["OrderId"]),
                OrderDate = Convert.ToDateTime(src["OrderDate"]), 
                Customer = null,
                dishes = null
            };
            return order;
        }
    }
}
