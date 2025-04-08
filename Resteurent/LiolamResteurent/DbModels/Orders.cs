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
        public string CustomerId { get; set; } //To change maybe
        public List<Dishes> dishes { get; set; }
        public Orders() { }
        public Orders(DateTime orderDate)
        {
            OrderDate = orderDate;
            CustomerId = "";
            this.dishes = null;
        }
    }
}
