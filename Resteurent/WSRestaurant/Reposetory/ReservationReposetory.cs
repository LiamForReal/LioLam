using System.Data;
using LiolamResteurent;

namespace WSRestaurant
{
    public class ReservationRerposetory : Reposetory, IReposetory<Reservations>
    {
        public ReservationRerposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Reservations model)
        {
            string sql = $@"INSERT INTO Reservations (ReserveDate, AmountOfPeople, ReserveHour) VALUSE (@ReserveDate, @AmountOfPeople, @ReserveHour)";
            this.dbContext.AddParameter("@ReserveDate", model.ReserveDate.ToString());
            this.dbContext.AddParameter("@AmountOfPeople", model.AmountOfPeople.ToString());
            this.dbContext.AddParameter("@ReserveHour", model.ReserveHour.ToString());
            return this.dbContext.Insert(sql);
        }

        public bool delete(string id)
        {
            string sql = $@"DELETE FROM Reservations WHERE ReservationId=@ReservationId";
            this.dbContext.AddParameter("@ReservationId", id);
            return this.dbContext.Delete(sql);
            
        }

        public List<Reservations> getAll()
        {
            List<Reservations> list = new List<Reservations>();
            string sql = "SELECT * FROM Reservations";
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createReservationObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Reservations getById(string id)
        {
            string sql = "SELECT FROM Reservations WHERE ReservationId = @ReservationId";
            this.dbContext.AddParameter("@ReservationId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                dataReader.Read();
                return this.modelFactory.createReservationObject.CreateModel(dataReader);
            }
        }
        public bool update(Reservations model)
        {
            string sql = $@"UPDATE Reservations SET ReserveDate = @ReserveDate, AmountOfPeople = @AmountOfPeople, ReserveHour = @ReserveHour WHERE ReserveId = @ReserveId";
            this.dbContext.AddParameter("@ReserveDate", model.ReserveDate.ToString());
            this.dbContext.AddParameter("@AmountOfPeople", model.AmountOfPeople.ToString());
            this.dbContext.AddParameter("@ReserveHour", model.ReserveHour.ToString());
            this.dbContext.AddParameter("@ReservationId", model.ReserveId.ToString());
            return this.dbContext.Update(sql);
        }

        public List<Reservations> GetByCustomer(string customerId)
        {
            List<Reservations> list = new List<Reservations>();
            string sql = "SELECT Reservations.ReserveId, Reservations.CustomerId, Reservations.ReserveDate, Reservations.ReserveHour, Reservations.PeopleAmount" +
                " FROM Reservations INNER JOIN Customers  ON Customers.CustomerId = Reservations.CustomerId" +
                " WHERE Customers.CustomerId = " + customerId + ";";
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createReservationObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Reservations GetByDate(DateTime Date, List<Reservations> reservations)
        {
            foreach(Reservations reservation in reservations)
            {
                if(reservation.ReserveDate == Date)
                    return reservation;
            }
            return null;
        }
    }
}
