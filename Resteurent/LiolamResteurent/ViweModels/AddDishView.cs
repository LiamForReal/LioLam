using LiolamResteurent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AddDishView
    {
        public List<Category> types { get; set; }
        public List<Chef> chefs { get; set; }
    }
}
