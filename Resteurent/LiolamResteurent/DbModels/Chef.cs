using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Chef : IModel
    {
        public string ChefFirstName { get; set; }
        public string ChefLastName { get; set; }
        public string ChefImage { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Chef chef)
            {
                return this.Id == chef.Id && this.ChefFirstName == chef.ChefFirstName && this.ChefLastName == chef.ChefLastName;
            }
            return false;
        }
    }
}
