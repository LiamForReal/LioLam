using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class StreetCreator : IModelCreator<Streets>
    {
        /// <summary>
        /// Creates a Street model from an IDataReader source.
        /// </summary>
        public Streets CreateModel(IDataReader src)
        {
            Streets street = new Streets()
            {
                Id = Convert.ToString(src["StreetId"]),
                StreetName = Convert.ToString(src["StreetName"]),
                //customers = null
            };
            return street;
        }
    }
}
