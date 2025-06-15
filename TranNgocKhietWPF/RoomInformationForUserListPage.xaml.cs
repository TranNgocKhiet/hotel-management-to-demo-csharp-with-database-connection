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
            
        private ObservableCollection<BookingDetail> pendingBookings = new ObservableCollection<BookingDetail>();
        private Customer currentCustomer;

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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRoomList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTextBox.Text.Trim().ToLower();

            try
            {
                var allRooms = iRoomInformationService.GetRoomInformations();

                var filteredRooms = allRooms
                    .Where(r => r.RoomNumber.ToLower().Contains(keyword))
                    .ToList();

                RoomDataGrid.ItemsSource = null;
                RoomDataGrid.ItemsSource = filteredRooms;
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
                RoomDataGrid.Items.Clear();
                var rooms = iRoomInformationService.GetRoomInformations();
                RoomDataGrid.ItemsSource = null;
                RoomDataGrid.ItemsSource = rooms;
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
            if (sender is Button button && button.Tag is RoomInformation room)
            {
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
                }
            }
            LoadPendingBookingRoomList();
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
                (b.EndDate.ToDateTime(TimeOnly.MinValue) - b.StartDate.ToDateTime(TimeOnly.MinValue)).Days
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
                BookingStatus = 1
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
