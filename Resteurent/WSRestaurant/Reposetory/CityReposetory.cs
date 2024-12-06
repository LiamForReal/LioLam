using System.Data;
using LiolamResteurent;

namespace WSRestaurant
{
    public class CityRerposetory : Reposetory, IReposetory<Cities>
    {
        public CityRerposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Cities model)
        {
            
            string sql = $@"INSERT INTO Cities (CityName) VALUES (@CityName)";
            this.dbContext.AddParameter("@ChefFirstName", model.CityName);
             return this.dbContext.Insert(sql);
            
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM Cities WHERE CityId=@CityId";
            this.dbContext.AddParameter("@CityId", id);
             return this.dbContext.Delete(sql);
        }

        public List<Cities> getAll()
        {
            List<Cities> list = new List<Cities>();
            string sql = "SELECT * FROM Cities";
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {
                    list.Add(this.modelFactory.createCityObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Cities getById(string id)
        {
            string sql = "SELECT FROM Cities WHERE CityId = @CityId";
            this.dbContext.AddParameter("@CityId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createCityObject.CreateModel(dataReader);
            }
        }
        public bool update(Cities model) 
        {
            string sql = $@"UPDATE Cities SET CityName = @CityName WHERE CityId == @CityId;";
            this.dbContext.AddParameter("@CityName", model.CityName);
            this.dbContext.AddParameter("@CityId", model.Id);
             return this.dbContext.Update(sql);
            
        }
        public Cities getByCustomer(string customerId)
        {
            string sql = "SELECT Cities.CityId, Cities.CityName" +
                " FROM Cities INNER JOIN Customers ON Cities.CityId = Customers.CityId " +
                " WHERE Customers.CustomerId=@CustomerId;";
            this.dbContext.AddParameter("@CustomerId", customerId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                if(dataReader.Read())
                    return this.modelFactory.createCityObject.CreateModel(dataReader);  
            }
            return null;
        }
    }
}
