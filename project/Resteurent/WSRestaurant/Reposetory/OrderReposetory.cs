using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using LiolamResteurent;
namespace WSRestaurant
{
    public class OrderRerposetory : Reposetory, IReposetory<Order>
    {
        public OrderRerposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Order model)
        {
            List<int> counts = new List<int>();
            List<Dish> dishes = new List<Dish>();
            string sql = $@"INSERT INTO Orders (CustomerId, OrderDate) VALUES (@CustomerId, @OrderDate)";
            this.dbContext.AddParameter("@CustomerId", model.CustomerId);
            this.dbContext.AddParameter("@OrderDate", model.OrderDate.ToString());
            bool ok = this.dbContext.Insert(sql);
            foreach(Dish dish in model.dishes)
            {
                if (dishes.IndexOf(dish) == -1)
                {
                    dishes.Add(dish);
                    counts.Add(model.dishes.Count(d => d.Equals(dish)));
                }
            }
            if (ok)
            {
                int i = 0;
                foreach (Dish dish in dishes)//TO fix
                {
                    sql = $@"INSERT INTO DishOrder (DishId, OrderId, Price, Quantity) VALUES (@DishId, @OrderId, @Price, @Quantity)";
                    this.dbContext.AddParameter("@DishId", dish.Id);
                    this.dbContext.AddParameter("@OrderDate", model.OrderDate.ToString());
                    this.dbContext.AddParameter("@Price", dish.DishPrice.ToString());
                    this.dbContext.AddParameter("@Quantity", counts.ToArray()[i].ToString());
                    if(!this.dbContext.Insert(sql))
                    {
                        throw new Exception("return false seconed");
                    }
                    i++;
                }
                return ok;
            }
            else throw new Exception("return false");
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
            string sql = "SELECT FROM Orders WHERE OrderId = @OrderId";
            this.dbContext.AddParameter("@OrderId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createOrderObject.CreateModel(dataReader);
            }
        }

        public Order getByCustomer(string customerId)
        {
            string sql = "SELECT FROM Orders WHERE CustomerId = @CustomerId";
            this.dbContext.AddParameter("@CustomerId", customerId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createOrderObject.CreateModel(dataReader);
            }
        }
        public bool update(Order model) //not in use
        {
            List<int> counts = new List<int>();
            List<Dish> dishes = new List<Dish>();
            string sql = $@"UPDATE Orders SET CustomerId = @CustomerId, OrderDate = @OrderDate WHERE OrderId == @OrderId";
            this.dbContext.AddParameter("@CustomerId", model.CustomerId);
            this.dbContext.AddParameter("@OrderDate", model.OrderDate.ToString());
            bool ok = this.dbContext.Update(sql);
            foreach (Dish dish in model.dishes)
            {
                if (dishes.IndexOf(dish) == -1)
                {
                    dishes.Add(dish);
                    counts.Add(model.dishes.Count(d => d.Equals(dish)));
                }
            }
            if (ok)
            {
                int i = 0;
                foreach (Dish dish in dishes)//TO fix
                {
                    sql = $@"UPDATE DishOrder OrderId = @OrderId, Price = @Price, Quantity = @Quantity WHERE (SELECT OrderId FROM DishOrder WHERE ORDER BY DishId LIMIT 1) = @DishId";
                    this.dbContext.AddParameter("@DishId", dish.Id);
                    this.dbContext.AddParameter("@OrderDate", model.OrderDate.ToString());
                    this.dbContext.AddParameter("@Price", dish.DishPrice.ToString());
                    this.dbContext.AddParameter("@Quantity", counts.ToArray()[i].ToString());
                    i++;
                }
                return ok;
            }
            else throw new Exception("return false");

        }

        public bool deleteByCustomer(string customerId)
        {
            Order order = this.getByCustomer(customerId);
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
