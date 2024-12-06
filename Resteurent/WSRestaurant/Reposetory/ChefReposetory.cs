using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using LiolamResteurent;

namespace WSRestaurant
{
    public class ChefReposetory : Reposetory, IReposetory<Chefs>
    {

        public ChefReposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Chefs model)
        {
            string sql = $@"INSERT INTO Chefs (ChefFirstName, ChefLastName, ChefImage) VALUES (@ChefFirstName, @ChefLastName, @ChefImage)";
            this.dbContext.AddParameter("@ChefFirstName", model.ChefFirstName);
            this.dbContext.AddParameter("@ChefLastName", model.ChefLastName);
            this.dbContext.AddParameter("@ChefImage", model.ChefImage);
            return this.dbContext.Insert(sql);
            
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM Chefs WHERE ChefId=@ChefId";
            this.dbContext.AddParameter("@ChefId", id);
            return this.dbContext.Delete(sql);
        }

        public List<Chefs> getAll()
        {
            List<Chefs> list = new List<Chefs>();
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

        public Chefs getById(string id)
        {
            string sql = "SELECT FROM Chefs WHERE ChefId = @ChefId";
            this.dbContext.AddParameter("@ChefId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.CreateChefObject.CreateModel(dataReader);
            }
        }
        public bool update(Chefs model)
        {
            string sql = $@"UPDATE Chefs SET ChefFirstName = @ChefFirstName, ChefLastName = @ChefLastName, ChefImage = @ChefImage WHERE ChefId == @ChefId;";
            this.dbContext.AddParameter("@ChefFirstName", model.ChefFirstName);
            this.dbContext.AddParameter("@ChefLastName", model.ChefLastName);
            this.dbContext.AddParameter("@ChefImage", model.ChefImage);
            this.dbContext.AddParameter("@ChefId", model.Id);
            return this.dbContext.Update(sql);
            
        }
        public List<Chefs> GetByDish(string dishId)
        {
            List<Chefs> list = new List<Chefs>();
            string sql = "SELECT Chefs.ChefId, Chefs.ChefFirstName, Chefs.ChefLastName, Chefs.ChefPicture" +
                " FROM Chefs INNER JOIN (Dishes INNER JOIN DishChef ON Dishes.DishId = DishChef.DishId) ON Chefs.ChefId = DishChef.ChefId" +
                " WHERE DishChef.DishId=@DishId;";
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
