using System.Data;

namespace WSRestaurant
{
    public interface IDBContext
    {
        void Open(); //open connection with db
        void Close(); //close connection with db
        IDataReader Read(string sql); //returns a table from db
        object ReadValue(string sql);//returns a object from db
        bool Insert(string sql); // insert to db
        bool Update(string sql); //update db
        bool Delete(string sql);//delete from db
        void Commit();//tranzaction command that allowe the changes to be done
        void Rollback();//tranzaction command that erase the changes
    }
}
