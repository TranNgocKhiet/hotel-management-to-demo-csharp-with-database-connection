using BusinessObjects;
using Repositories;
using Services;
using System;
using System.Windows;
using System.Windows.Controls;

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
        }

        public BookingReservationDialog(BookingReservation booking) : this()
        {
            txtBookingDate.Text = booking.BookingDate.HasValue
                ? booking.BookingDate.Value.ToString("dd/MM/yyyy") : "";


            txtTotalPrice.Text = booking.TotalPrice.ToString();
            cbStatus.SelectedIndex = (int) booking.BookingStatus;
            txtCustomerID.Text = booking.CustomerID.ToString();

            BookingReservation = booking;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var bookingDate = DateOnly.Parse(txtBookingDate.Text);
                var totalPrice = decimal.Parse(txtTotalPrice.Text);
                var customerId = Int32.Parse(txtCustomerID.Text);

                if (cbStatus.SelectedItem is not ComboBoxItem selectedItem)
                {
                    MessageBox.Show("Please select a booking status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var status = Byte.Parse(selectedItem.Tag.ToString());

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
