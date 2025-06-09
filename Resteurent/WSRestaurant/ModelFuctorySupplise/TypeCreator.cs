using Models;
using System.Data;

namespace RestaurantWebService
{
    public class TypeCreator : IModelCreator<Category>
    {
        /// <summary>
        /// Creates a Type model from an IDataReader source.
        /// </summary>
        public Category CreateModel(IDataReader src)
        {
            Category type = new Category()
            {
                Id = Convert.ToString(src["TypeId"]), 
                TypeName = Convert.ToString(src["TypeName"]), 
            };
            return type;
        }
    }
}
