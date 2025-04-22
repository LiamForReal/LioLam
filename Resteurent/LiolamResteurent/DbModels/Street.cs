using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Street : IModel
    {
        public string StreetName { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Street street)
                return this.Id == street.Id && this.StreetName == street.StreetName;
            return false;
        }
    }
}
