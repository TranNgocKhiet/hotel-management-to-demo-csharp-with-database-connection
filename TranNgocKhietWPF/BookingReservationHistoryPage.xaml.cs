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

        private Customer currentCustomer;

        public BookingReservationHistoryPage(Customer customer)
        {
            InitializeComponent();

            currentCustomer = customer;
            var detailRepo = new BookingDetailRepository();
            iBookingDetailService = new BookingDetailService(detailRepo);

            var bookingReservationRepository = BookingReservationRepository.Instance;
            var bookingReservationService = new BookingReservationService(bookingReservationRepository);
            iBookingReservationService = bookingReservationService;
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
                    var details = iBookingDetailService.GetBookingDetails()
                        .Where(d => d.BookingReservationID == selectedBooking.BookingReservationID)
                        .ToList();

                    BookingDetailDataGrid.ItemsSource = null;
                    BookingDetailDataGrid.Items.Clear();
                    BookingDetailDataGrid.ItemsSource = details;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load booking details.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
