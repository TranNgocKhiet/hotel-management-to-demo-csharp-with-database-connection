using BusinessObjects;
using System;
using System.Globalization;
using System.Windows;

namespace TranNgocKhietWPF
{
    public partial class BookingDetailDialog : Window
    {
        public BookingDetail BookingDetail { get; private set; }

        public BookingDetailDialog()
        {
            InitializeComponent();
            BookingDetail = new BookingDetail();
        }

        public BookingDetailDialog(int bookingReservationID) : this()
        {
            txtBookingReservationID.Text = bookingReservationID.ToString();
            txtBookingReservationID.IsEnabled = false; 
            BookingDetail.BookingReservationID = bookingReservationID;
        }

        public BookingDetailDialog(BookingDetail detail) : this()
        {
            BookingDetail = detail;
            txtBookingReservationID.Text = detail.BookingReservationID.ToString();
            txtRoomID.Text = detail.RoomID.ToString();
            txtStartDate.Text = detail.StartDate?.ToString("yyyy-MM-dd") ?? "";
            txtEndDate.Text = detail.EndDate?.ToString("yyyy-MM-dd") ?? "";
            txtActualPrice.Text = detail.ActualPrice?.ToString() ?? "";
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BookingDetail.BookingReservationID = int.Parse(txtBookingReservationID.Text.Trim());
                BookingDetail.RoomID = int.Parse(txtRoomID.Text.Trim());
                BookingDetail.StartDate = DateOnly.ParseExact(txtStartDate.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                BookingDetail.EndDate = DateOnly.ParseExact(txtEndDate.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                BookingDetail.ActualPrice = decimal.Parse(txtActualPrice.Text.Trim());

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid input: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
