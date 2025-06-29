using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;
using Repositories;

namespace TranNgocKhietWPF
{
    public partial class BookingReservationReportPage : Page
    {
        private readonly IBookingReservationService bookingService;
        private readonly ICustomerService customerService;

        public class BookingReservationReportViewModel
        {
            public int BookingReservationID { get; set; }
            public DateOnly? BookingDate { get; set; }
            public string? CustomerFullName { get; set; }
            public decimal? TotalPrice { get; set; }
        }

        public BookingReservationReportPage()
        {
            InitializeComponent();
            BookingReservationRepository bookingReservationRepository = new BookingReservationRepository();
            bookingService = new BookingReservationService(bookingReservationRepository);

            CustomerRepository customerRepository = new CustomerRepository();
            customerService = new CustomerService(customerRepository);
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select both Start Date and End Date.");
                return;
            }

            DateOnly start = DateOnly.FromDateTime(StartDatePicker.SelectedDate.Value);
            DateOnly end = DateOnly.FromDateTime(EndDatePicker.SelectedDate.Value);

            if (start > end)
            {
                MessageBox.Show("Start Date must be before or equal to End Date.");
                return;
            }

            List<BookingReservation> reservations = bookingService.GetBookingReservations();
            List<Customer> customers = customerService.GetCustomers();

           
            try
            {
                var filtered = reservations
                   .Where(r => r.BookingDate >= start && r.BookingDate <= end)
                   .Join(customers,
                   r => r.CustomerID,
                   c => c.CustomerID,
                   (r, c) => new BookingReservationReportViewModel
                   {
                       BookingReservationID = r.BookingReservationID,
                       BookingDate = r.BookingDate,
                       CustomerFullName = c.CustomerFullName,
                       TotalPrice = r.TotalPrice
                   })
                   .OrderByDescending(r => r.BookingDate)
                   .ToList();

                ReportDataGrid.ItemsSource = filtered;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate report.\n" + ex.Message);
            }
        }
    }
}
