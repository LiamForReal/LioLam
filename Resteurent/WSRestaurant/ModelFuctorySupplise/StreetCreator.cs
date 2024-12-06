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
                StreetId = Convert.ToInt32(src["StreetId"]),
                StreetName = Convert.ToString(src["StreetName"]),
                customers = null
            };
            return street;
        }
    }
}
