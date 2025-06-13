using BusinessObjects;

namespace DataAccessLayer
{
    public class BookingDetailDAO
    {
        public static List<BookingDetail> GetBookingDetails()
        {
            var bookingDetails = new List<BookingDetail>();
            try
            {
                using var db = new MyHotelContext();
                bookingDetails = db.BookingDetails.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return bookingDetails;
        }

        public static BookingDetail GetBookingDetail(int id)
        {
            using var db = new MyHotelContext();
            return db.BookingDetails.FirstOrDefault(c => c.BookingReservationID.Equals(id));
        }

        public static void AddBookingDetail(BookingDetail bookingDetail)
        {
            try
            {
                using var context = new MyHotelContext();
                context.BookingDetails.Add(bookingDetail);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateBookDetail(BookingDetail bookingDetail)
        {
            try
            {
                using var db = new MyHotelContext();
                db.Entry<BookingDetail>(bookingDetail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges(); ;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void RemoveBookDetail(BookingDetail bookingDetail)
        {
            try
            {
                using var db = new MyHotelContext();
                var b = db.BookingDetails.SingleOrDefault(b => b.BookingReservationID == bookingDetail.BookingReservationID);
                db.BookingDetails.Remove(b);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
