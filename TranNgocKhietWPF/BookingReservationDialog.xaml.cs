using BusinessObjects;
using Repositories;
using Services;
using System;
using System.Windows;

namespace TranNgocKhietWPF
{
    public partial class BookingReservationDialog : Window
    {
        public BookingReservation BookingReservation { get; private set; }

        private readonly ICustomerService iCustomerService;

        public BookingReservationDialog()
        {
            InitializeComponent();

            var customerRepo = new CustomerRepository();
            iCustomerService = new CustomerService(customerRepo);

            LoadCustomers();
        }

        public BookingReservationDialog(BookingReservation booking) : this()
        {
            txtBookingDate.Text = booking.BookingDate.HasValue
                ? booking.BookingDate.Value.ToString("dd/MM/yyyy") : "";


            txtTotalPrice.Text = booking.TotalPrice.ToString();
            txtStatus.Text = booking.BookingStatus.ToString();
            cboCustomer.SelectedValue = booking.CustomerID;

            BookingReservation = booking;
        }

        private void LoadCustomers()
        {
            var customers = iCustomerService.GetCustomers();
            cboCustomer.ItemsSource = customers;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var bookingDate = DateOnly.Parse(txtBookingDate.Text);
                var totalPrice = decimal.Parse(txtTotalPrice.Text);
                var status = Byte.Parse(txtStatus.Text);
                var customerId = (int)cboCustomer.SelectedValue;

                if (BookingReservation == null)
                {
                    BookingReservation = new BookingReservation();
                }

                BookingReservation.BookingDate = bookingDate;
                BookingReservation.TotalPrice = totalPrice;
                BookingReservation.BookingStatus = status;
                BookingReservation.CustomerID = customerId;

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid input: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
