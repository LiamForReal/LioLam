using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Types : Models
    {
        public string TypeName { get; set; }
        public List<Dishes> dishes { get; set; }

        public Types() { }
        public Types(string typeName)
        {
            TypeName = typeName;
            this.dishes = null;
        }
    }
}
