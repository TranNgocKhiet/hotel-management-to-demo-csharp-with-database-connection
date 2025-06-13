using BusinessObjects;

namespace DataAccessLayer
{
    public class RoomTypeDAO
    {
        public static List<RoomType> GetRoomTypes()
        {
            var roomTypes = new List<RoomType>();
            try
            {
                using var db = new MyHotelContext();
                roomTypes = db.RoomTypes.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return roomTypes;
        }

        public static RoomType GetRoomType(int id)
        {
            using var db = new MyHotelContext();
            return db.RoomTypes.FirstOrDefault(r => r.RoomTypeID == id);
        }

        public static void AddRoomType(RoomType roomType)
        {
            try
            {
                using var db = new MyHotelContext();
                db.RoomTypes.Add(roomType);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateRoomType(RoomType roomType)
        {
            try
            {
                using var db = new MyHotelContext();
                db.Entry<RoomType>(roomType).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void RemoveRoomType(RoomType roomType)
        {
            try
            {
                using var db = new MyHotelContext();
                var r = db.RoomTypes.SingleOrDefault(r => r.RoomTypeID == roomType.RoomTypeID);
                if (r != null)
                {
                    db.RoomTypes.Remove(r);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
