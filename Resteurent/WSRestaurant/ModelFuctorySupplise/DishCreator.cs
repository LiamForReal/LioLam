using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class DishCreator : IModelCreator<Dish>
    {
        /// <summary>
        /// Creates a Dish model from an IDataReader source.
        /// </summary>
        public Dish CreateModel(IDataReader src)
        {
            Dish dish = new Dish()
            {
                Id = Convert.ToString(src["DishId"]),
                DishDescription = Convert.ToString(src["DishDescription"]),
                DishName = Convert.ToString(src["DishName"]),
                DishPrice = Convert.ToInt32(src["DishPrice"]),
                DishImage = "http://localhost:5125/Images/Dishes/" + Convert.ToString(src["DishImage"]),
                chefs = null, 
                types = null,
                
                // orders = null,
            };
            return dish;
        }
    }
}
