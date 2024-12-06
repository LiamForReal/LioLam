using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Streets : Models
    {
        public string StreetName { get; set; }
        public List<Customers> customers { get; set; }

        public Streets() { }
        public Streets(string streetName)
        {
            this.StreetName = streetName;
        }

    }
}
