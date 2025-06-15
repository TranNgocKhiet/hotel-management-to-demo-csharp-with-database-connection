using BusinessObjects;

namespace DataAccessLayer
{
    public class RoomInformationDAO
    {
        public static List<RoomInformation> GetRoomInformations()
        {
            var rooms = new List<RoomInformation>();
            try
            {
                using var db = new MyHotelContext();
                rooms = db.RoomInformations.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return rooms;
        }

        public static RoomInformation GetRoomInformation(int id)
        {
            using var db = new MyHotelContext();
            return db.RoomInformations.FirstOrDefault(r => r.RoomID == id);
        }

        public static void AddRoomInformation(RoomInformation room)
        {
            try
            {
                using var db = new MyHotelContext();
                db.RoomInformations.Add(room);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateRoomInformation(RoomInformation room)
        {
            try
            {
                using var db = new MyHotelContext();
                db.Entry<RoomInformation>(room).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void RemoveRoomInformation(RoomInformation room)
        {
            try
            {
                using var db = new MyHotelContext();
                List<BookingDetail> bookingDetails = db.BookingDetails.Where(b => b.RoomID == room.RoomID).ToList();
                var rooms = db.RoomInformations.SingleOrDefault(r => r.RoomID == room.RoomID);

                if (rooms != null)
                {
                    foreach (var roomInUse in bookingDetails)
                    {
                        if (roomInUse.RoomID == room.RoomID)
                        {
                            room.RoomStatus = 0;
                            db.RoomInformations.Update(room);
                        }
                        else
                        {
                            db.RoomInformations.Remove(rooms);
                            db.SaveChanges();
                        }
                    } 
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
