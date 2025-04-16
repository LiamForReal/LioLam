using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using LiolamResteurent;

namespace WSRestaurant
{
    public class TypeReposetory : Reposetory, IReposetory<Category>
    {

        public TypeReposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Category model)
        {
            string sql = $@"INSERT INTO Types (TypeName) VALUES (@TypeName)";
            this.dbContext.AddParameter("@TypeName", model.TypeName);
            return this.dbContext.Insert(sql);
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM DishType WHERE TypeId=@TypeId;";
            this.dbContext.AddParameter("@TypeId", id);
            if (this.dbContext.Delete(sql))
            {
                sql = $@"DELETE FROM Types WHERE TypeId=@TypeId";
                this.dbContext.AddParameter("@TypeId", id);
                return this.dbContext.Delete(sql);
            }
            else throw new Exception("get false");
        }

        public List<Category> getAll()
        {
            List<Category> list = new List<Category>();
            string sql = "SELECT * FROM Types";
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createTypeObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Category getById(string id)
        {
            string sql = "SELECT * FROM Types WHERE TypeId = @TypeId";
            this.dbContext.AddParameter("@TypeId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createTypeObject.CreateModel(dataReader);
            }
        }
        public bool update(Category model)
        {
            string sql = $@"UPDATE Types SET TypeName = @TypeName WHERE TypeId == @TypeId;";
            this.dbContext.AddParameter("@TypeName", model.TypeName);
            this.dbContext.AddParameter("@TypeId", model.Id);
            return this.dbContext.Update(sql);
        }
        
        public List<Category> getByDish(string dishId)
        {
            List<Category> list = new List<Category>();
            string sql = "SELECT Types.TypeId, Types.TypeName" +
                " FROM Types INNER JOIN (Dishes INNER JOIN DishType ON Dishes.DishId = DishType.DishId) ON Types.TypeId = DishType.TypeId" +
                " WHERE DishType.DishId=@DishId;";
            this.dbContext.AddParameter("@DishId", dishId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createTypeObject.CreateModel(dataReader));
                }
            }
            return list;
        }
    }
}
