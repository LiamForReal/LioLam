using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Orders : Models
    {
        public DateTime OrderDate { get; set; }
        public Customers Customer { get; set; }
        public List<Dishes> dishes { get; set; }
    }
}
