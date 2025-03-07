using Microsoft.AspNetCore.Connections.Features;
using System.Data;
using System.Data.OleDb;
using System.Reflection.PortableExecutable;

namespace WSRestaurant
{
    public class DBContext : IDBContext
    {
        private OleDbCommand command; // Responsible for executing SQL commands
        private OleDbConnection connection; // Manages the database connection (open and close)
        private OleDbTransaction transaction; // Manages transactions (commit and rollback)
        private static DBContext dbContext; // Singleton instance of DBContext

        // Singleton pattern for DBContext: ensures a single instance is used.
        //tools -> nugget -> middle one -> search oledb -> make it only on wsResteurant 
        public static DBContext GetInstance()
        {
            if (dbContext == null)
                dbContext = new DBContext();
            return dbContext;
        }

        // Constructor initializes connection and command objects
        private DBContext()
        {
            this.connection = new OleDbConnection();
            this.connection.ConnectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Directory.GetCurrentDirectory()}\App_Data\ResteurantDB.accdb";
            this.command = new OleDbCommand();
            this.command = this.connection.CreateCommand();
        }

        /// <summary>
        /// Opens the database connection.
        /// </summary>
        public void Open()
        {
            this.connection.Open();
        }

        /// <summary>
        /// Closes the database connection and disposes of the transaction, if it exists.
        /// </summary>
        public void Close()
        {
            this.connection.Close();
            if (this.transaction != null)
            {
                this.transaction.Dispose();
            }
            clearCollection();
        }

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        public void Commit()
        {
            this.transaction.Commit();
        }

        public void BeginTransaction()
        {
            this.transaction = this.connection.BeginTransaction();
            this.command.Transaction = this.transaction;
        }


        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        public void Rollback()
        {
            this.transaction.Rollback();
        }

        /// <summary>
        /// Executes a SQL delete command.
        /// </summary>
        /// <param name="sql">SQL delete command.</param>
        /// <returns>True if rows were affected, false otherwise.</returns>
        public bool Delete(string sql)
        {
            bool flag = executeSql(sql);
            return flag;
        }

        /// <summary>
        /// Executes a SQL insert command.
        /// </summary>
        /// <param name="sql">SQL insert command.</param>
        /// <returns>True if rows were affected, false otherwise.</returns>
        public bool Insert(string sql)
        {
            bool flag = executeSql(sql);
            return flag;
        }

        /// <summary>
        /// Executes a SQL select command and returns a data reader.
        /// </summary>
        /// <param name="sql">SQL select command.</param>
        /// <returns>IDataReader containing the query results.</returns>
        public IDataReader Read(string sql)
        {
            this.command.CommandText = sql;
            IDataReader reader =  this.command.ExecuteReader();
            clearCollection();
            return reader;
        }

        /// <summary>
        /// Executes a SQL select command and returns a single value.
        /// </summary>
        /// <param name="sql">SQL select command.</param>
        /// <returns>Object containing the result of the query.</returns>
        public object ReadValue(string sql)
        {
            this.command.CommandText = sql;
            object obj = this.command.ExecuteScalar();
            clearCollection();
            return obj;
        }


        /// <summary>
        /// Executes a SQL update command.
        /// </summary>
        /// <param name="sql">SQL update command.</param>
        /// <returns>True if rows were affected, false otherwise.</returns>
        public bool Update(string sql)
        {
            bool flag = executeSql(sql);
            return flag;
        }

        /// <summary>
        /// adding new parameter to the collection of parameters
        /// </summary>
        /// <param name="paramName">parameter Name string type</param>
        /// <param name="paramValue">parameter value string type/param>
        public void AddParameter(string paramName, string paramValue)
        {
            this.command.Parameters.Add(new OleDbParameter(paramName, paramValue));
        }

        /// <summary>
        /// only clears all the parameters in the parameters list
        /// </summary>
        public void clearCollection()
        {
            this.command.Parameters.Clear();
        }
        /// <summary>
        /// Helper function to execute non-query SQL commands (insert, update, delete).
        /// </summary>
        /// <param name="sql">SQL command to execute.</param>
        /// <returns>True if rows were affected, false otherwise.</returns>
        public bool executeSql(string sql)
        {
            this.command.CommandText = sql;
            Console.WriteLine(this.command.CommandText);
            bool flag = this.command.ExecuteNonQuery() > 0;
            clearCollection();
            return flag;
        }

       
       



    }
}
