using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderView
    {
        public Order order { get; set; }

        public int delivery { get; set; }

        public OrderView()
        {
            order = new Order();
            delivery = 0;
        }
    }
}
