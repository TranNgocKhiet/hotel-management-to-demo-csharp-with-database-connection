using BusinessObjects;
using Repositories;
using Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TranNgocKhietWPF
{
    public partial class RoomInformationForUserListPage : Page
    {
        private readonly IRoomInformationService iRoomInformationService;
        private readonly IBookingDetailService iBookingDetailService;
        private readonly IBookingReservationService iBookingReservationService;
        private readonly IRoomTypeService iRoomTypeService;

        private ObservableCollection<BookingDetail> pendingBookings = new ObservableCollection<BookingDetail>();
        private Customer currentCustomer;

        public class RoomDisplayDto
        {
            public int RoomID { get; set; }
            public string? RoomNumber { get; set; }
            public string? RoomDetailDescription { get; set; }
            public int? RoomMaxCapacity { get; set; }
            public int? RoomStatus { get; set; }
            public decimal? RoomPricePerDay { get; set; }

            public string? RoomTypeName { get; set; }
        }

        public RoomInformationForUserListPage(Customer customer)
        {
            InitializeComponent();

            currentCustomer = customer;

            var roomRepository = new RoomInformationRepository();
            iRoomInformationService = new RoomInformationService(roomRepository);

            var detailRepository = new BookingDetailRepository();
            iBookingDetailService = new BookingDetailService(detailRepository);

            var reservationRepository = new BookingReservationRepository();
            iBookingReservationService = new BookingReservationService(reservationRepository);

            var roomTypeRepository = new RoomTypeRepository();
            iRoomTypeService = new RoomTypeService(roomTypeRepository);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRoomList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTextBox.Text.Trim().ToLowerInvariant();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadRoomList();
                if (BookingDataGrid.ItemsSource == null)
                    BookingDataGrid.ItemsSource = pendingBookings;
                return;
            }

            try
            {
                var rooms = iRoomInformationService.GetRoomInformations();
                var roomTypes = iRoomTypeService.GetRoomTypes();

                var displayList = rooms
                    .Select(r => new RoomDisplayDto
                    {
                        RoomID = r.RoomID,
                        RoomNumber = r.RoomNumber,
                        RoomDetailDescription = r.RoomDetailDescription,
                        RoomMaxCapacity = r.RoomMaxCapacity,
                        RoomStatus = r.RoomStatus,
                        RoomPricePerDay = r.RoomPricePerDay,
                        RoomTypeName = roomTypes.FirstOrDefault(rt => rt.RoomTypeID == r.RoomTypeID)?.RoomTypeName ?? "Unknown"
                    })
                    .Where(dto =>
                           (dto.RoomNumber?.ToLowerInvariant().Contains(keyword) ?? false) ||
                           (dto.RoomTypeName?.ToLowerInvariant().Contains(keyword) ?? false) ||
                           (dto.RoomDetailDescription?.ToLowerInvariant().Contains(keyword) ?? false))
                    .ToList();

                RoomDataGrid.ItemsSource = displayList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while searching room information");
            }
        }


        private void LoadRoomList()
        {
            try
            {
                var rooms = iRoomInformationService.GetRoomInformations();
                var roomTypes = iRoomTypeService.GetRoomTypes(); 

                var displayList = rooms.Select(r => new RoomDisplayDto
                {
                    RoomID = r.RoomID,
                    RoomNumber = r.RoomNumber,
                    RoomDetailDescription = r.RoomDetailDescription,
                    RoomMaxCapacity = r.RoomMaxCapacity,
                    RoomStatus = r.RoomStatus,
                    RoomPricePerDay = r.RoomPricePerDay,
                    RoomTypeName = roomTypes.FirstOrDefault(rt => rt.RoomTypeID == r.RoomTypeID)?.RoomTypeName ?? "Unknown"
                }).ToList();

                RoomDataGrid.ItemsSource = displayList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading room information");
            }
        }

        private void LoadPendingBookingRoomList()
        {
            BookingDataGrid.ItemsSource = null;
            BookingDataGrid.ItemsSource = pendingBookings;
        }

        private void BookRoomButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is RoomDisplayDto roomDto)
            {
                var room = iRoomInformationService.GetRoomInformations()
                            .FirstOrDefault(r => r.RoomID == roomDto.RoomID);

                if (room == null)
                {
                    MessageBox.Show("Room not found.");
                    return;
                }

                var dialog = new BookingDialog(room);
                if (dialog.ShowDialog() == true)
                {
                    var booking = new BookingDetail
                    {
                        RoomID = room.RoomID,
                        StartDate = dialog.StartDate,
                        EndDate = dialog.EndDate,
                        ActualPrice = room.RoomPricePerDay,
                    };

                    pendingBookings.Add(booking);
                    LoadPendingBookingRoomList();
                }
            }
        }


        private void ConfirmBookingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (pendingBookings.Count == 0)
            {
                MessageBox.Show("No bookings to confirm.");
                return;
            }

            DateTime dateTime = DateTime.Now; 
            DateOnly dateOnly = DateOnly.FromDateTime(dateTime);

            var totalPrice = pendingBookings.Sum(b =>
                b.ActualPrice *
                ((b.EndDate.ToDateTime(TimeOnly.MinValue) - b.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1)
            );

            int ID = 1;
            List<BookingReservation> existingReservations = iBookingReservationService.GetBookingReservations();
            foreach (var reservation in existingReservations)
            {
                if (reservation.BookingReservationID >= ID)
                {
                    ID = reservation.BookingReservationID + 1;
                }
            }

            var bookingReservation = new BookingReservation
            {
                BookingReservationID = ID,
                BookingDate = dateOnly,
                TotalPrice = totalPrice,
                CustomerID = currentCustomer.CustomerID,
                BookingStatus = 0
            };

            iBookingReservationService.AddBookingReservation(bookingReservation);

            foreach (var booking in pendingBookings)
            {
                booking.BookingReservationID = bookingReservation.BookingReservationID;
                iBookingDetailService.AddBookingDetail(booking);
            }

            MessageBox.Show("Bookings confirmed successfully.");
            pendingBookings.Clear();
        }
    }
}
