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
        private Customer currentCustomer;

        public BookingReservationHistoryPage(Customer customer)
        {
            InitializeComponent();

            currentCustomer = customer;

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
    }
}
