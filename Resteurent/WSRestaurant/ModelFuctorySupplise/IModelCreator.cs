using System.Data;

namespace RestaurantWebService
{
    public interface IModelCreator<T>
    {
        T CreateModel(IDataReader src); //create model pure fuction
    }
}
