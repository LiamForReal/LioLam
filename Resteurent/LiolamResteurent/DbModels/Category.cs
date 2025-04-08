using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Category : Model
    {
        public string TypeName { get; set; }
        public Category(string typeName)
        {
            TypeName = typeName;
        }

        public Category()
        {
        }
    }
}
