using System.Data;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using LiolamResteurent;

namespace WSRestaurant
{
    public class DishRerposetory : Reposetory, IReposetory<Dish>
    {
        public DishRerposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Dish model)
        {
            string sql = $@"INSERT INTO Dishes (DishDescription, DishName, DishPrice, DishImage) VALUES (@DishDescription, @DishName, @DishPrice, @DishImage)";
            this.dbContext.AddParameter("@DishDescription", model.DishName);
            this.dbContext.AddParameter("@DishName", model.DishName);
            this.dbContext.AddParameter("@DishPrice", model.DishName);
            this.dbContext.AddParameter("@DishImage", model.DishName);
            bool ok = this.dbContext.Insert(sql);
            if (ok)
            {
                throw new Exception("return false");
            }
            foreach(Category type in model.types)
            {
                sql = $@"INSERT INTO DishType (DishId, TypeId) VALUES(@DishId, @TypeId)";
                this.dbContext.AddParameter("@DishId", model.Id);
                this.dbContext.AddParameter("@TypeId", type.Id);
                if (!this.dbContext.Insert(sql))
                {
                    throw new Exception("return false seconed");
                }
            }

            foreach (Chef chef in model.chefs)
            {
                sql = $@"INSERT INTO DishChef (DishId, ChefId) VALUES(@DishId, @ChefId)";
                this.dbContext.AddParameter("@DishId", model.Id);
                this.dbContext.AddParameter("@ChefId", chef.Id);
                if (!this.dbContext.Insert(sql))
                {
                    throw new Exception("return false seconed");
                }
            }
            return ok;
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM DishType WHERE DishId=@DishId";
            this.dbContext.AddParameter("@DishId", id);
            bool flagType = this.dbContext.Delete(sql);

            sql = $@"DELETE FROM DishChef WHERE DishId=@DishId";
            this.dbContext.AddParameter("@DishId", id);
            bool flagChef = this.dbContext.Delete(sql);

            sql = $@"DELETE FROM DishOrder WHERE DishId=@DishId"; //ajust to order
            this.dbContext.AddParameter("@DishId", id);
            bool flagOrder = this.dbContext.Delete(sql);
            if (flagOrder && flagType && flagChef)
            {
                sql = $@"DELETE FROM Dishes WHERE DishId=@DishId";
                this.dbContext.AddParameter("@DishId", id);
                return this.dbContext.Delete(sql);
            }
            else throw new Exception("return false");
            
        }

        public List<Dish> getAll()
        {
            List<Dish> list = new List<Dish>();
            string sql = "SELECT * FROM Dishes";
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createDishObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Dish getById(string id)
        {
            string sql = "SELECT * FROM Dishes WHERE DishId = @DishId";
            this.dbContext.AddParameter("@DishId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createDishObject.CreateModel(dataReader);
            }
        }
        public bool update(Dish model)
        {
            string sql = $@"UPDATE Dishes SET DishDescription = @DishDescription, DishName = @DishName, DishPrice = @DishPrice, DishImage = @DishImage WHERE DishId = @DishId";
            this.dbContext.AddParameter("@DishName", model.DishName);
            this.dbContext.AddParameter("@DishDescription", model.DishName);
            this.dbContext.AddParameter("@DishPrice", model.DishName);
            this.dbContext.AddParameter("@DishImage", model.DishName);
            this.dbContext.AddParameter("@DishId", model.Id);
            bool ok = this.dbContext.Update(sql);
            if (ok)
                throw new Exception("return false");

            sql = "DELETE FROM DishType WHERE DishId = @DishId";
            this.dbContext.AddParameter("@DishId", model.Id);
            if (!this.dbContext.Insert(sql))
                throw new Exception("return false seconed");

            foreach (Category type in model.types)
            {
                sql = $@"INSERT INTO DishType (DishId, TypeId) VALUES(@DishId, @TypeId)";
                this.dbContext.AddParameter("@DishId", model.Id);
                this.dbContext.AddParameter("@TypeId", type.Id);
                if (!this.dbContext.Insert(sql))
                {
                    throw new Exception("return false seconed");
                }
            }

            sql = "DELETE FROM DishChef WHERE DishId = @DishId";
            this.dbContext.AddParameter("@DishId", model.Id);
            if (!this.dbContext.Insert(sql))
                throw new Exception("return false seconed");

            foreach (Chef chef in model.chefs)
            {
                sql = $@"INSERT INTO DishChef (DishId, ChefId) VALUES(@DishId, @ChefId)";
                this.dbContext.AddParameter("@DishId", model.Id);
                this.dbContext.AddParameter("@ChefId", chef.Id);
                if (!this.dbContext.Insert(sql))
                {
                    throw new Exception("return false seconed");
                }
            }
            return ok;
        }

        public List<Dish> GetByOrder(string OrderId)
        {
            List<Dish> list = new List<Dish>();
            string sql = "SELECT Dishes.DishName, Dishes.DishDescription, Dishes.DishPrice, Dishes.DishImage, Dishes.DishId, Orders.OrderId" +
                " FROM Orders INNER JOIN (Dishes INNER JOIN DishOrder ON Dishes.DishId = DishOrder.DishId) ON Orders.OrderId = DishOrder.OrderId" +
                " WHERE Orders.OrderId=@OrderId;";
            this.dbContext.AddParameter("@OrderId", OrderId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createDishObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public List<Dish> GetByChef(string chefId)
        {
            List<Dish> list = new List<Dish>();
            string sql = "SELECT Dishes.DishId, Dishes.DishName, Dishes.DishDescription, Dishes.DishPrice, Dishes.DishImage" +
                " FROM Chefs INNER JOIN (Dishes INNER JOIN DishChef ON Dishes.DishId = DishChef.DishId) ON Chefs.ChefId = DishChef.ChefId" +
                " WHERE chefs.ChefId=@ChefId;";
            this.dbContext.AddParameter("@ChefId", chefId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createDishObject.CreateModel(dataReader));
                }
            }
            return list;
        }
        public List<Dish> GetByType(string typefId)
        {
            List<Dish> list = new List<Dish>();
            string sql = "SELECT Dishes.DishId, Dishes.DishName, Dishes.DishDescription, Dishes.DishPrice, Dishes.DishImage" +
                " FROM Types INNER JOIN (Dishes INNER JOIN DishType ON Dishes.DishId = DishType.DishId) ON Types.TypeId = DishType.TypeId" +
                " WHERE Types.TypeId=@TypeId;";
            this.dbContext.AddParameter("@TypeId", typefId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {
                    list.Add(this.modelFactory.createDishObject.CreateModel(dataReader));
                }
            }
            return list;
        }
    }
}
