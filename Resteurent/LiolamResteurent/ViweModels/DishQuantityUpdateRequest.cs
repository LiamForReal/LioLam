using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DishQuantityUpdateRequest
    {
        public int DishId { get; set; }
        public int Quantity { get; set; }
    }
}
