using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class City : IModel
    {
        public string CityName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is City city)
            {
                return this.Id == city.Id && this.CityName == city.CityName;
            }
            return false;
        }
    }
}
