using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class StreetCreator : IModelCreator<Street>
    {
        /// <summary>
        /// Creates a Street model from an IDataReader source.
        /// </summary>
        public Street CreateModel(IDataReader src)
        {
            Street street = new Street()
            {
                Id = Convert.ToString(src["StreetId"]),
                StreetName = Convert.ToString(src["StreetName"])
                //customers = null
            };
            return street;
        }
    }
}
