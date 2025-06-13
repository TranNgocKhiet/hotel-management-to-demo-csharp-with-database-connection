using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("BookingDetail", Schema = "dbo")]
    public partial class BookingDetail
    {
        [Key]
        public int BookingReservationID { get; set; }
        public int RoomID { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public Decimal? ActualPrice { get; set; }

        public BookingDetail() { }

        public BookingDetail(int bookingReservationID, int roomID,
            DateOnly? startDate, DateOnly? endDate, Decimal? actualPrice)
        {
            BookingReservationID = bookingReservationID;
            RoomID = roomID;
            StartDate = startDate;
            EndDate = endDate;
            ActualPrice = actualPrice;
        }
    }
}
