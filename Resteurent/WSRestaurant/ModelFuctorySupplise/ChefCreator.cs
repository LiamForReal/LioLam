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
                Id = Convert.ToString(src["ChefId"]),
                ChefFirstName = Convert.ToString(src["ChefFirstName"]),
                ChefLastName = Convert.ToString(src["ChefLastName"]),
                ChefImage = "http://localhost:5125/Images/Chefs/" + Convert.ToString(src["ChefPicture"]),
            };
            return chef;
        }
    }
}
