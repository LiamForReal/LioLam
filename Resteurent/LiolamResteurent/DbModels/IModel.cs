using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiolamResteurent
{
    public class IModel
    {
        public string? Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is IModel other)
                return this.Id == other.Id;

            return false;
        }
    }
}
