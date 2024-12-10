using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Chefs : Models
    {
        public string ChefFirstName { get; set; }
        public string ChefLastName { get; set; }
        public string ChefImage { get; set; }

        public Chefs() { }
        public Chefs(string chefFirstName, string chefLastName, string chefImage)
        {
            ChefFirstName = chefFirstName;
            ChefLastName = chefLastName;
            ChefImage = chefImage;
        }
    }
}
