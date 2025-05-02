using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using LiolamResteurent;
using Models;
namespace WSRestaurant
{
    public class OrderRerposetory : Reposetory, IReposetory<Order>
    {
        public OrderRerposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Order model)
        {
            string sql = $@"INSERT INTO Orders (CustomerId, OrderDate) VALUES (@CustomerId, @OrderDate)";
            this.dbContext.AddParameter("@CustomerId", model.CustomerId);
            this.dbContext.AddParameter("@OrderDate", model.OrderDate.ToString());
            bool ok = this.dbContext.Insert(sql);
            if (ok)
            {
                foreach (OrderProduct product in model.products)
                {
                    sql = $@"INSERT INTO DishOrder (DishId, OrderId, Price, Quantity) VALUES (@DishId, @OrderId, @Price, @Quantity)";
                    this.dbContext.AddParameter("@DishId", product.Id);
                    this.dbContext.AddParameter("@OrderId", model.Id);
                    this.dbContext.AddParameter("@Price", product.totalPrice.ToString());
                    this.dbContext.AddParameter("@Quantity", product.Quatity.ToString());
                    if (!this.dbContext.Insert(sql))
                    {
                        throw new Exception("return false seconed");
                    }
                }
                return ok;
            }
            else throw new Exception("return false");
        }

        public string getLastId()
        {
            string sql = $"SELECT MAX(OrderId) FROM Orders";
            string orderId = this.dbContext.ReadValue(sql)?.ToString();
            return (int.Parse(orderId) + 1).ToString();
        }

        //public bool addNewDishToOrder()
        public bool delete(string id)
        {
            string sql = $@"DELETE FROM Order WHERE OrderId=@OrderId";
            this.dbContext.AddParameter("@OrderId", id);
            return this.dbContext.Delete(sql);
        }

        public List<Order> getAll()
        {
            List<Order> list = new List<Order>();
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

        public Order getById(string id)
        {
            string sql = "SELECT * FROM Orders WHERE OrderId = @OrderId";
            this.dbContext.AddParameter("@OrderId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createOrderObject.CreateModel(dataReader);
            }
        }

        public List<string> getByCustomer(string customerId)
        {
            List<string> ordersId = new List<string>();
            string sql = "SELECT * FROM Orders WHERE CustomerId = @CustomerId";
            this.dbContext.AddParameter("@CustomerId", customerId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {
                    string id = Convert.ToString(dataReader["OrderId"]);
                    ordersId.Add(id);
                }
            }
            return ordersId;
        }

        public List<OrderProduct> getOrderProducts(string orderId)
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            string sql = "SELECT * FROM DishOrder WHERE OrderId = @OrderId";
            this.dbContext.AddParameter("@OrderId", orderId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {
                    OrderProduct product = modelFactory.CreateOrderProductObject.CreateModel(dataReader);
                    orderProducts.Add(product);
                }
            }
            return orderProducts;
        }
        public bool update(Order model) //not in use
        {
            List<int> counts = new List<int>();
            List<Dish> dishes = new List<Dish>();
            string sql = $@"UPDATE Orders SET Payed = @Payed WHERE OrderId = @OrderId";
            this.dbContext.AddParameter("@OrderId", model.Id);
            return this.dbContext.Update(sql);
            //to change
        }
    }
}
