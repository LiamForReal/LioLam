using LiolamResteurent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderProduct : IModel
    {//Id is dishId
        public int Quatity { get; set; }
        public string Image { get; set; } 
        public string Name { get; set; }
        public int overAllPrice { get; set; }
        
    }
}
