using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using LiolamResteurent;

namespace WSRestaurant
{
    public class TypeReposetory : Reposetory, IReposetory<Types>
    {

        public TypeReposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Types model)
        {
            string sql = $@"INSERT INTO Types (TypeName) VALUES (@TypeName)";
            this.dbContext.AddParameter("@TypeName", model.TypeName);
            return this.dbContext.Insert(sql);
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM Types WHERE TypeId=@TypeId";
            this.dbContext.AddParameter("@TypeId", id);
            return this.dbContext.Delete(sql);
        }

        public List<Types> getAll()
        {
            List<Types> list = new List<Types>();
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

        public Types getById(string id)
        {
            string sql = "SELECT FROM Types WHERE TypeId = @TypeId";
            this.dbContext.AddParameter("@TypeId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createTypeObject.CreateModel(dataReader);
            }
        }
        public bool update(Types model)
        {
            string sql = $@"UPDATE Types SET TypeName = @TypeName WHERE TypeId == @TypeId;";
            this.dbContext.AddParameter("@TypeName", model.TypeName);
            this.dbContext.AddParameter("@TypeId", model.Id);
            return this.dbContext.Update(sql);
        }
        
        public List<Types> getByDish(string dishId)
        {
            List<Types> list = new List<Types>();
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
