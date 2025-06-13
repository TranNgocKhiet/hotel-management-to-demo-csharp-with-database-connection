using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObjects
{
    [Table("RoomInformation", Schema = "dbo")]
    public partial class RoomInformation
    {
        [Key] 
        public int RoomID { get ; set; }
        public string? RoomNumber { get; set; }
        public string? RoomDetailDescription { get; set; }
        public int? RoomMaxCapacity { get; set; }
        public int RoomTypeID { get; set; }
        public Byte? RoomStatus { get; set; }
        public Decimal? RoomPricePerDay { get; set; }

        public RoomInformation() { }

        public RoomInformation(int roomID, string? roomNumber, string? roomDescription, 
            int? roomMaxCapacity, int roomTypeID, Byte? roomStatus, Decimal? roomPricePerDate)
        {
            RoomID = roomID;
            RoomNumber = roomNumber;
            RoomDetailDescription = roomDescription;
            RoomMaxCapacity = roomMaxCapacity;
            RoomStatus = roomStatus;
            RoomPricePerDay = roomPricePerDate;
            RoomTypeID = roomTypeID;
        }
    }
}