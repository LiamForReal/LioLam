using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Menu
    {
        public List<Dishes> Dishes { get; set; }
        public List<Types> Types { get; set; }
        public List<Chefs> Chefs { get; set; }

        public int totalPages { get; set; }
        public int PageNumber { get; set; }
        public int ChefId { get; set; }
        public int TypeId { get; set; }
    }
}
