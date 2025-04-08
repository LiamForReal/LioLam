using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Street : Model
    {
        public string StreetName { get; set; }
        public Street(string streetName)
        {
            this.StreetName = streetName;
        }

        public Street()
        {
        }
    }
}
