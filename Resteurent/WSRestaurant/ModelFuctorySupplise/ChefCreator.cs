using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class ChefCreator : IModelCreator<Chef>
    {
        /// <summary>
        /// Creates a Chef model from an IDataReader source.
        /// </summary>
        public Chef CreateModel(IDataReader src)
        {
            
            Chef chef = new Chef()
            {
                Id = Convert.ToString(src["ChefId"]),
                ChefFirstName = Convert.ToString(src["ChefFirstName"]),
                ChefLastName = Convert.ToString(src["ChefLastName"]),
                ChefImage = "https://localhost:5125/Images/Chefs/" + Convert.ToString(src["ChefImage"]),
            };
            return chef;
        }
    }
}
