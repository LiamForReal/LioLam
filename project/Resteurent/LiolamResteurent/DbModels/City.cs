using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class City : Model
    {
        public string CityName { get; set; }
        public City() { }
        public City(string cityName)
        {
            this.CityName = cityName;
        }
    }
}
