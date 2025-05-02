using LiolamResteurent;
using Models;
using System.Data;
using WSRestaurant;

namespace RestaurantWebSerice
{
    public class OrderProductCreator : IModelCreator<OrderProduct>
    {
        public OrderProduct CreateModel(IDataReader src)
        {
            OrderProduct product = new OrderProduct()
            {
                Id = Convert.ToString(src["DishId"]),
                Quatity = Convert.ToInt32(src["Quantity"]),
                totalPrice = Convert.ToInt32(src["Price"]),
                Name = "", 
                Image = ""
            };
 
            return product;
        }
    }
}
