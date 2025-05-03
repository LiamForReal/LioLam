namespace WSRestaurant
{
    public class UnitOfWorkReposetory
    {
        ChefReposetory chefReposetory;
        CityRerposetory cityRerposetory;
        CustomerRerposetory customerRerposetory;
        DishRerposetory dishRerposetory;
        OrderRerposetory orderRerposetory;
        StreetReposetory streetReposetory;
        TypeReposetory typeReposetory;

        DBContext dbContext;

        public UnitOfWorkReposetory(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public ChefReposetory chefRepositoryObject
        {
            get
            {
                if (this.chefReposetory == null)
                    this.chefReposetory = new ChefReposetory(dbContext);
                return this.chefReposetory;
            }
        }
        public CityRerposetory cityRerposetoryObject
        {
            get
            {
                if (this.cityRerposetory == null)
                    this.cityRerposetory = new CityRerposetory(dbContext);
                return this.cityRerposetory;
            }
        }
        public CustomerRerposetory customerRerposetoryObject
        {
            get
            {
                if (this.customerRerposetory == null)
                    this.customerRerposetory = new CustomerRerposetory(dbContext);
                return this.customerRerposetory;
            }
        }
        public DishRerposetory dishRerposetoryObject
        {
            get
            {
                if (this.dishRerposetory == null)
                    this.dishRerposetory = new DishRerposetory(dbContext);
                return this.dishRerposetory;
            }
        }
        public OrderRerposetory orderRerposetoryObject
        {
            get
            {
                if (this.orderRerposetory == null)
                    this.orderRerposetory = new OrderRerposetory(dbContext);
                return this.orderRerposetory;
            }
        }
        public StreetReposetory streetReposetoryObject
        {
            get
            {
                if (this.streetReposetory == null)
                    this.streetReposetory = new StreetReposetory(dbContext);
                return this.streetReposetory;
            }
        }
        public TypeReposetory typeReposetoryObject
        {
            get
            {
                if (this.typeReposetory == null)
                    this.typeReposetory = new TypeReposetory(dbContext);
                return this.typeReposetory;
            }
        }
    }
}
