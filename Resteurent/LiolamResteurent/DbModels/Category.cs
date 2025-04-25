using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class Category : IModel
    {
        public string TypeName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Category type)
            {
                return this.Id == type.Id && this.TypeName == type.TypeName;
            }
            return false;
        }
    }
}
