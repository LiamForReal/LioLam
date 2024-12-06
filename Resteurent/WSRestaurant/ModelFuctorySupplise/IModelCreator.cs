using System.Data;

namespace WSRestaurant
{
    public interface IModelCreator<T>
    {
        T CreateModel(IDataReader src); //create model pure fuction
    }
}
