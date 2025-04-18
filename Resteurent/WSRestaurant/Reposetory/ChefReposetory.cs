using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using LiolamResteurent;

namespace WSRestaurant
{
    public class ChefReposetory : Reposetory, IReposetory<Chef>
    {

        public ChefReposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Chef model)
        {
            string sql = $@"INSERT INTO Chefs (ChefFirstName, ChefLastName, ChefImage) VALUES (@ChefFirstName, @ChefLastName, @ChefImage)";
            this.dbContext.AddParameter("@ChefFirstName", model.ChefFirstName);
            this.dbContext.AddParameter("@ChefLastName", model.ChefLastName);
            this.dbContext.AddParameter("@ChefImage", model.ChefImage);
            bool flag = this.dbContext.Insert(sql);

            if (!flag)
            {
                throw new Exception("return false");
            }

            model.Id = GetLastId();
            model.ChefImage = $"{model.Id}{model.ChefImage}";
            sql = $@"UPDATE Chefs SET ChefImage = @ChefImage WHERE ChefId = @ChefId";
            this.dbContext.AddParameter("@ChefImage", model.ChefImage);
            this.dbContext.AddParameter("@ChefId", model.Id);
            if (!this.dbContext.Update(sql))
            {
                throw new Exception("return false");
            }
            return flag;
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM DishChef WHERE ChefId=@ChefId";
            this.dbContext.AddParameter("@ChefId", id);
            this.dbContext.Delete(sql);

            sql = $@"DELETE FROM Chefs WHERE ChefId=@ChefId";
            this.dbContext.AddParameter("@ChefId", id);
            return this.dbContext.Delete(sql);
        }

        public List<Chef> getAll()
        {
            List<Chef> list = new List<Chef>();
            string sql = "SELECT * FROM Chefs";
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {
                    
                    list.Add(this.modelFactory.CreateChefObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Chef getById(string id)
        {
            string sql = "SELECT * FROM Chefs WHERE ChefId = @ChefId";
            this.dbContext.AddParameter("@ChefId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.CreateChefObject.CreateModel(dataReader);
            }
        }

        public Chef getByName(string firstName, string lastName)
        {
            string sql = "SELECT * FROM Chefs WHERE ChefFirstName = @ChefFirstName AND ChefLastName = @ChefLastName";
            this.dbContext.AddParameter("@ChefFirstName", firstName);
            this.dbContext.AddParameter("@ChefLastName", lastName);

            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.CreateChefObject.CreateModel(dataReader);
            }
        }
        public bool update(Chef model)
        {
            string sql = $@"UPDATE Chefs SET ChefFirstName = @ChefFirstName, ChefLastName = @ChefLastName, ChefImage = @ChefImage WHERE ChefId = @ChefId;";
            this.dbContext.AddParameter("@ChefFirstName", model.ChefFirstName);
            this.dbContext.AddParameter("@ChefLastName", model.ChefLastName);
            this.dbContext.AddParameter("@ChefImage", model.ChefImage);
            this.dbContext.AddParameter("@ChefId", model.Id);
            return this.dbContext.Update(sql);
            
        }
        public List<Chef> GetByDish(string dishId)
        {
            List<Chef> list = new List<Chef>();
            string sql = "SELECT Chefs.ChefId, Chefs.ChefLastName, Chefs.ChefFirstName, Chefs.ChefImage FROM Chefs INNER JOIN DishChef ON Chefs.ChefId = DishChef.ChefId WHERE DishChef.DishId = @DishId;";
            this.dbContext.AddParameter("@DishId", dishId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {
                    list.Add(this.modelFactory.CreateChefObject.CreateModel(dataReader));
                }
            }
            return list;
        }
    }
}
