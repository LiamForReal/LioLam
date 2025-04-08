using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class CityCreator : IModelCreator<City>
    {
        /// <summary>
        /// Creates a City model from an IDataReader source.
        /// </summary>
        public City CreateModel(IDataReader src)
        {
            City city = new City()
            {
                Id = Convert.ToString(src["CityId"]),
                CityName = Convert.ToString(src["CityName"])
                //customers = null
            };
            return city;
        }
    }
}
