using LiolamResteurent;
using System.Data;

namespace WSRestaurant
{
    public class TypeCreator : IModelCreator<Types>
    {
        /// <summary>
        /// Creates a Type model from an IDataReader source.
        /// </summary>
        public Types CreateModel(IDataReader src)
        {
            Types type = new Types()
            {
                TypeId = Convert.ToInt32(src["TypeId"]), 
                TypeName = Convert.ToString(src["TypeName"]), 
                dishes = null
            };
            return type;
        }
    }
}
