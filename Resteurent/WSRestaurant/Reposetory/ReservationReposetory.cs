using System.Data;
using LiolamResteurent;

namespace WSRestaurant
{
    public class ReservationRerposetory : Reposetory, IReposetory<Reservation>
    {
        public ReservationRerposetory(DBContext dbContext) : base(dbContext) { }
        public bool create(Reservation model)
        {
            string sql = $@"INSERT INTO Reservations (CustomerId, ReserveDate, AmountOfPeople, ReserveHour) VALUSE (@CustomerId, @ReserveDate, @AmountOfPeople, @ReserveHour)";
            this.dbContext.AddParameter("@CustomerId", model.CustomerId);
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

        public bool deleteByCustomer(string CustomerId)
        {
            string sql = $@"DELETE FROM Reservations WHERE CustomerId=@CustomerId";
            this.dbContext.AddParameter("@CustomerId", CustomerId);
            return this.dbContext.Delete(sql);

        }

        public List<Reservation> getAll()
        {
            List<Reservation> list = new List<Reservation>();
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

        public Reservation getById(string id)
        {
            string sql = "SELECT FROM Reservations WHERE ReservationId = @ReservationId";
            this.dbContext.AddParameter("@ReservationId", id);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                if(dataReader.Read())
                    return this.modelFactory.createReservationObject.CreateModel(dataReader);
            }
            return null;
        }
        public bool update(Reservation model)
        {
            string sql = $@"UPDATE Reservations SET CustomerId = @CustomerId, ReserveDate = @ReserveDate, AmountOfPeople = @AmountOfPeople, ReserveHour = @ReserveHour WHERE ReserveId = @ReserveId";
            this.dbContext.AddParameter("@CustomerId", model.CustomerId);
            this.dbContext.AddParameter("@ReserveDate", model.ReserveDate.ToString());
            this.dbContext.AddParameter("@AmountOfPeople", model.AmountOfPeople.ToString());
            this.dbContext.AddParameter("@ReserveHour", model.ReserveHour.ToString());
            this.dbContext.AddParameter("@ReservationId", model.Id);
            return this.dbContext.Update(sql);
        }

        public List<Reservation> GetByCustomer(string customerId)
        {
            List<Reservation> list = new List<Reservation>();
            string sql = "SELECT Reservations.ReserveId, Reservations.CustomerId, Reservations.ReserveDate, Reservations.ReserveHour, Reservations.PeopleAmount" +
                " FROM Reservations INNER JOIN Customers  ON Customers.CustomerId = Reservations.CustomerId" +
                " WHERE Customers.CustomerId = @CustomerId;";
            this.dbContext.AddParameter("@CustomerId", customerId);
            using (IDataReader dataReader = this.dbContext.Read(sql))
            {
                while (dataReader.Read())
                {

                    list.Add(this.modelFactory.createReservationObject.CreateModel(dataReader));
                }
            }
            return list;
        }

        public Reservation GetByDate(DateTime Date, List<Reservation> reservations)
        {
            foreach(Reservation reservation in reservations)
            {
                if(reservation.ReserveDate == Date)
                    return reservation;
            }
            return null;
        }
    }
}
