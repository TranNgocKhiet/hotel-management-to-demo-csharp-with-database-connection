using Repositories;
using Services;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;

namespace TranNgocKhietWPF
{
    public partial class BookingReservationHistoryPage : Page
    {
        private readonly IBookingReservationService iBookingReservationService;
        private readonly IBookingDetailService iBookingDetailService;
        private readonly IRoomInformationService iRoomInformationService;
        private Customer currentCustomer;

        public class BookingDetailDisplayDto
        {
            public int BookingReservationID { get; set; }
            public int? RoomID { get; set; }
            public string? RoomNumber { get; set; }    
            public DateOnly? StartDate { get; set; }
            public DateOnly? EndDate { get; set; }
            public decimal? ActualPrice { get; set; }
        }

        public BookingReservationHistoryPage(Customer customer)
        {
            InitializeComponent();

            currentCustomer = customer;

            BookingDetailRepository detailRepo = new BookingDetailRepository();
            iBookingDetailService = new BookingDetailService(detailRepo);

            BookingReservationRepository bookingReservationRepository = BookingReservationRepository.Instance;
            iBookingReservationService = new BookingReservationService(bookingReservationRepository);

            RoomInformationRepository roomInformationRepository = RoomInformationRepository.Instance;
            iRoomInformationService= new RoomInformationService(roomInformationRepository);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBookingReservationList();
        }

        public void LoadBookingReservationList()
        {
            try
            {
                var bookingReservations = iBookingReservationService.GetBookingReservationsByCustomerID(currentCustomer.CustomerID);
                BookingReservationDataGrid.ItemsSource = bookingReservations;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on load list of booking reservation");
            }
        }

        private void BookingDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingReservationDataGrid.SelectedItem is BookingReservation selectedBooking)
            {
                try
                {
                    var allRooms = iRoomInformationService.GetRoomInformations();
                    var details = iBookingDetailService.GetBookingDetails()
                        .Where(d => d.BookingReservationID == selectedBooking.BookingReservationID)
                        .ToList();

                    var mergedList = details.Select(d =>
                    {
                        var room = allRooms.FirstOrDefault(r => r.RoomID == d.RoomID);
                        return new BookingDetailDisplayDto
                        {
                            BookingReservationID = d.BookingReservationID,
                            RoomID = d.RoomID,
                            RoomNumber = room?.RoomNumber ?? "Unknown",
                            StartDate = d.StartDate,
                            EndDate = d.EndDate,
                            ActualPrice = d.ActualPrice
                        };
                    }).ToList();

                    BookingDetailDataGrid.ItemsSource = null;
                    BookingDetailDataGrid.Items.Clear();
                    BookingDetailDataGrid.ItemsSource = mergedList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load booking details.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
