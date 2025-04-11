using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Order : Model
    {
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; } //To change maybe
        public List<Dish> dishes { get; set; }
        public Order(DateTime orderDate)
        {
            OrderDate = orderDate;
            CustomerId = "";
            this.dishes = null;
        }

        public Order()
        {
        }
    }
}
