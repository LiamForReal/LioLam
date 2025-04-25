using Models;
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
        public List<OrderProduct> products { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Order order)
                return order.Id == this.Id && order.OrderDate == this.OrderDate && this.products.Equals(order);
            return false;
        }
    }
}
