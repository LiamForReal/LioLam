namespace WSRestaurant
{
    public class Reposetory
    {
        protected DBContext dbContext;
        protected ModelFactory modelFactory;

        public Reposetory(DBContext dbContext)
        {
            this.dbContext = dbContext;
            this.modelFactory = new ModelFactory();
        }

        public string GetLastId()
        {
            string sql = "SELECT @@Identity";
            return this.dbContext.ReadValue(sql).ToString();
        }
    }
}
