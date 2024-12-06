using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class ChefCreator : IModelCreator<Chefs>
    {
        /// <summary>
        /// Creates a Chef model from an IDataReader source.
        /// </summary>
        public Chefs CreateModel(IDataReader src)
        {
            
            Chefs chef = new Chefs()
            {
                ChefId = Convert.ToInt32(src["ChefId"]),
                ChefFirstName = Convert.ToString(src["ChefFirstName"]),
                ChefLastName = Convert.ToString(src["ChefLastName"]),
                ChefImage = Convert.ToString(src["ChefImage"]),
                DishesMade = null
            };
            return chef;
        }
    }
}
