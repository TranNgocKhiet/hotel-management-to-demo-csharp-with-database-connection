using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("BookingReservation", Schema = "dbo")]
    public partial class BookingReservation
    {
        [Key]
        public int BookingReservationID { get; set; }
        public DateOnly? BookingDate { get; set; }
        public Decimal? TotalPrice { get; set; }
        public int CustomerID { get; set; }
        public Byte? BookingStatus { get; set; }

        public BookingReservation() { }

        public BookingReservation(int bookingReservationID,
            DateOnly? bookingDate, Decimal? totalPrice, int customerID, Byte? bookingStatus)
        {
            BookingReservationID = bookingReservationID;
            BookingDate = bookingDate;
            TotalPrice = totalPrice;
            CustomerID = customerID;
            BookingStatus = bookingStatus;
        }
    }
}
