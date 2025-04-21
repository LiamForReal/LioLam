using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Customer : IModel
    {
        public string CustomerUserName { get; set; }
        public int CustomerHouse { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerMail { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerImage { get; set; }
        public bool IsOwner { get; set;}

        public City city { get; set; }
        public Street street { get; set; }
    }
}
