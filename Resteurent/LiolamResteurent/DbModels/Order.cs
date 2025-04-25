using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Order : IModel
    {
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; } //To change maybe
        public List<Dish> dishes { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Order order)
                return order.Id == this.Id && order.OrderDate == this.OrderDate && this.dishes.Equals(order);
            return false;
        }
    }
}
