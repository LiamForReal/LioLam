using System.Data;
using LiolamResteurent;

namespace WSRestaurant
{
    public class OrderRerposetory : Reposetory, IReposetory<Orders>
    {
        public OrderRerposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Orders model)
        {
            string sql = $@"INSERT INTO Orders (OrderDate) VALUES (@OrderDate)";
            this.dbContext.AddParameter("@OrderDescription", model.OrderDate.ToString());
            return this.dbContext.Insert(sql);
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM Orders WHERE OrderId=@OrderId";
            this.dbContext.AddParameter("@OrderId", id);
            return this.dbContext.Delete(sql);
        }

        public List<Orders> getAll()
        {
            List<Orders> list = new List<Orders>();
            string sql = "SELECT * FROM Orders";
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createOrderObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Orders getById(string id)
        {
            string sql = "SELECT FROM Orders WHERE OrderId = @OrderId";
            this.dbContext.AddParameter("@OrderId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createOrderObject.CreateModel(dataReader);
            }
        }
        public bool update(Orders model)
        {
            string sql = $@"UPDATE Orders SET OrderDate = @OrderDate WHERE OrderId == @OrderId";
            this.dbContext.AddParameter("@OrderName", model.OrderDate.ToString());
            this.dbContext.AddParameter("@OrderId", model.OrderId.ToString());
            return this.dbContext.Update(sql);
        }
    }
}
