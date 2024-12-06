using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class DishCreator : IModelCreator<Dishes>
    {
        /// <summary>
        /// Creates a Dish model from an IDataReader source.
        /// </summary>
        public Dishes CreateModel(IDataReader src)
        {
            Dishes dish = new Dishes()
            {
                Id = Convert.ToString(src["DishId"]),
                DishDescription = Convert.ToString(src["DishDescription"]),
                DishName = Convert.ToString(src["DishName"]),
                DishPrice = Convert.ToInt32(src["DishPrice"]),
                DishImage = Convert.ToString(src["DishImage"]),
               // orders = null,
               // chefs = null, 
               // types = null
            };
            return dish;
        }
    }
}
