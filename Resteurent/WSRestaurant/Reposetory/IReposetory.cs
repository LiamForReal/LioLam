namespace WSRestaurant
{
    public interface IReposetory<T>
    {
        List<T> getAll();
        T getById(string id);
        bool create(T model); 
        bool update(T model);
        bool delete(string id);
    }
}
