using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using LiolamResteurent;

namespace WSRestaurant
{
    public class StreetReposetory : Reposetory, IReposetory<Streets>
    {

        public StreetReposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Streets model)
        {
            string sql = $@"INSERT INTO Streets (StreetName) VALUES (@StreetName)";
            this.dbContext.AddParameter("@StreetName", model.StreetName);
            return this.dbContext.Insert(sql);    
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM Streets WHERE StreetId=@StreetId";
            this.dbContext.AddParameter("@StreetId", id);
            return this.dbContext.Delete(sql);
        }

        public List<Streets> getAll()
        {
            List<Streets> list = new List<Streets>();
            string sql = "SELECT * FROM Streets";
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createStreetObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Streets getById(string id)
        {
            string sql = "SELECT FROM Streets WHERE StreetId = @StreetId";
            this.dbContext.AddParameter("@StreetId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createStreetObject.CreateModel(dataReader);
            }
        }
        public bool update(Streets model)
        {
            string sql = $@"UPDATE Streets SET StreetName = @StreetName WHERE StreetId == @StreetId;";
            this.dbContext.AddParameter("@StreetName", model.StreetName);
            this.dbContext.AddParameter("@StreetId", model.Id);
            return this.dbContext.Update(sql);
        }
    }
}
