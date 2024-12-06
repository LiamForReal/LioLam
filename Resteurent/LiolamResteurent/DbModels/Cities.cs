using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Cities : Models
    {
        public string CityName { get; set; }
        public List<Customers> customers { get; set; }
        public Cities() { }
        public Cities(string cityName)
        {
            this.CityName = cityName;
            customers = null;
        }
    }
}
