using System.Data;
using LiolamResteurent;
using Microsoft.AspNetCore.Identity;

namespace WSRestaurant
{
    public class CustomerRerposetory : Reposetory, IReposetory<Customer>
    {
        public CustomerRerposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Customer model)
        {
            string sql = $@"INSERT INTO Customers (CustomerId, CustomerUserName, CustomerHouse,CityId, StreetId, CustomerPhone, CustomerMail, CustomerPassword, CustomerImage, IsOwner) 
                            VALUES ('{model.Id}', '{model.CustomerUserName}', '{model.CustomerHouse}',{model.city.Id},{model.street.Id}, '{model.CustomerPhone}',
                                    '{model.CustomerMail}','{model.CustomerPassword}', '{model.CustomerImage}', {false})";
            bool flag = this.dbContext.Insert(sql);
            return flag;
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM Customers WHERE CustomerId=@CustomerId";
            this.dbContext.AddParameter("@CustomerId", id);
            return this.dbContext.Delete(sql);

        }

        public List<Customer> getAll()
        {
            List<Customer> list = new List<Customer>();
            string sql = "SELECT * FROM Customers";
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createCustomerObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Customer getById(string id)
        {
            string sql = "SELECT * FROM Customers WHERE CustomerId = @CustomerId";
            this.dbContext.AddParameter("@CustomerId", id);

            //
            //($"sql is: {sql}, id is: {id}");
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createCustomerObject.CreateModel(dataReader);
            }
        }
        public bool update(Customer model)
        {
            string sql = $@"UPDATE Customers SET CustomerUserName = @CustomerUserName, CustomerHouse =  @CustomerHouse,CityId = @CityId, StreetId = @StreetId ,CustomerPhone =  @CustomerPhone" +
                 ", CustomerMail = @CustomerMail, CustomerPassword = @CustomerPassword, CustomerImage = @CustomerImage WHERE CustomerId = @CustomerId;";
            this.dbContext.AddParameter("@CustomerUserName", model.CustomerUserName);
            this.dbContext.AddParameter("@CustomerHouse", model.CustomerHouse.ToString());
            this.dbContext.AddParameter("@CityId", model.city.Id);
            this.dbContext.AddParameter("@StreetId", model.street.Id);
            this.dbContext.AddParameter("@CustomerPhone", model.CustomerPhone);
            this.dbContext.AddParameter("@CustomerMail", model.CustomerMail);
            this.dbContext.AddParameter("@CustomerPassword", model.CustomerPassword);
            this.dbContext.AddParameter("@CustomerImage", model.CustomerImage);
            this.dbContext.AddParameter("@CustomerId", model.Id);
            return this.dbContext.Update(sql);
        }

        public string CheckIfAdmin(string userName, string password)
        {
            string sql = @"SELECT * FROM Customers WHERE Customers.CustomerUserName=@CustomerUserName AND Customers.IsOwner = true;";

            this.dbContext.AddParameter("@CustomerUserName", userName);

            Customer customer;
            try
            {
                using (IDataReader dataReader = this.dbContext.Read(sql))
                {
                    dataReader.Read();
                    customer = this.modelFactory.createCustomerObject.CreateModel(dataReader);
                }
                if (BCrypt.Net.BCrypt.Verify(password, customer.CustomerPassword))
                    return customer.Id;
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        internal Customer getByName(string userName)
        {
            string sql = "SELECT * FROM Customers WHERE CustomerUserName = @CustomerUserName";
            this.dbContext.AddParameter("@CustomerUserName", userName);

            //($"sql is: {sql}, id is: {id}");
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createCustomerObject.CreateModel(dataReader);
            }
        }

        internal string login(string userName, string password)
        {
            string sql = "SELECT * FROM Customers WHERE CustomerUserName = @CustomerUserName";
            this.dbContext.AddParameter("@CustomerUserName", userName);
            //($"sql is: {sql}, id is: {id}");
            try
            {
                Customer customer;

                using (IDataReader dataReader = this.dbContext.Read(sql))
                {
                    dataReader.Read();
                    customer = this.modelFactory.createCustomerObject.CreateModel(dataReader);
                }
                if (BCrypt.Net.BCrypt.Verify(password, customer.CustomerPassword)) // check if the password and the already exisiting password are mached
                    return customer.Id;
                return null;
            }
            catch(Exception e)
            {
                return "";
            }
        }
    }
}
