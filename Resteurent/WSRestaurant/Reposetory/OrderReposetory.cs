using System.Data;
using LiolamResteurent;

namespace WSRestaurant
{
    public class OrderRerposetory : Reposetory, IReposetory<Orders>
    {
        public OrderRerposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Orders model)
        {
            string sql = $@"INSERT INTO Orders (CustomerId, OrderDate) VALUES (@CustomerId, @OrderDate)";
            this.dbContext.AddParameter("@CustomerId", model.Customer.Id);
            this.dbContext.AddParameter("@OrderDescription", model.OrderDate.ToString());
            return this.dbContext.Insert(sql);
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE DishOrder WHERE OrderId=@OrderId";
            this.dbContext.AddParameter("@OrderId", id);
            if (this.dbContext.Delete(sql))
            {
                sql = $@"DELETE FROM Orders WHERE OrderId=@OrderId";
                this.dbContext.AddParameter("@OrderId", id);
                return this.delete(sql);
            }
            else throw new Exception("return false");
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

        public Orders getByCustomer(string customerId)
        {
            string sql = "SELECT FROM Orders WHERE CustomerId = @CustomerId";
            this.dbContext.AddParameter("@CustomerId", customerId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createOrderObject.CreateModel(dataReader);
            }
        }
        public bool update(Orders model)
        {
            string sql = $@"UPDATE Orders SET CustomerId = @CustomerId, OrderDate = @OrderDate WHERE OrderId == @OrderId";
            this.dbContext.AddParameter("@CustomerId", model.Customer.Id);
            this.dbContext.AddParameter("@OrderDate", model.OrderDate.ToString());
            return this.dbContext.Update(sql);
        }

        public bool deleteByCustomer(string customerId)
        {
            Orders order = this.getByCustomer(customerId);
            string sql = $@"DELETE FROM Orders WHERE CustomerId=@CustomerId";
            this.dbContext.AddParameter("@CustomerId", customerId);
            if (this.dbContext.Delete(sql))
            {
                sql = $@"DELETE FROM DishOrder WHERE OrderId=@OrderId";
                this.dbContext.AddParameter("@DishId", order.Id);
                return this.dbContext.Delete(sql);
            }
            else throw new Exception("return false");
        }
    }
}
