using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class CityCreator : IModelCreator<Cities>
    {
        /// <summary>
        /// Creates a City model from an IDataReader source.
        /// </summary>
        public Cities CreateModel(IDataReader src)
        {
            Cities city = new Cities()
            {
                Id = Convert.ToString(src["CityId"]),
                CityName = Convert.ToString(src["CityName"]),
                //customers = null
            };
            return city;
        }
    }
}
