using BusinessObjects;

namespace DataAccessLayer
{
    public class BookingReservationDAO
    {
        public static List<BookingReservation> GetBookingReservations()
        {
            var reservations = new List<BookingReservation>();
            try
            {
                using var db = new MyHotelContext();
                reservations = db.BookingReservations.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return reservations;
        }

        public static BookingReservation GetBookingReservation(int id)
        {
            using var db = new MyHotelContext();
            return db.BookingReservations.FirstOrDefault(r => r.BookingReservationID == id);
        }

        public static void AddBookingReservation(BookingReservation reservation)
        {
            try
            {
                using var db = new MyHotelContext();
                db.BookingReservations.Add(reservation);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateBookingReservation(BookingReservation reservation)
        {
            try
            {
                using var db = new MyHotelContext();
                db.Entry<BookingReservation>(reservation).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void RemoveBookingReservation(BookingReservation reservation)
        {
            try
            {
                using var db = new MyHotelContext();
                var r = db.BookingReservations.SingleOrDefault(r => r.BookingReservationID == reservation.BookingReservationID);
                if (r != null)
                {
                    db.BookingReservations.Remove(r);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static List<BookingReservation> GetBookingReservationsByCustomerID(int customerId)
        {
            try
            {
                using var db = new MyHotelContext();
                return db.BookingReservations.Where(r => r.CustomerID == customerId).ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
