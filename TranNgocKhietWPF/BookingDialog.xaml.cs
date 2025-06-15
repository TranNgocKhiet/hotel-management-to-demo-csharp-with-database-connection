using BusinessObjects;
using System;
using System.Windows;

namespace TranNgocKhietWPF
{
    public partial class BookingDialog : Window
    {
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }

        public BookingDialog(RoomInformation room)
        {
            InitializeComponent();
            Title = $"Book Room {room.RoomNumber}";
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select both start and end dates.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime start = StartDatePicker.SelectedDate.Value;
            DateTime end = EndDatePicker.SelectedDate.Value;

            if (end < start)
            {
                MessageBox.Show("End date must be after start date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            StartDate = DateOnly.FromDateTime(start);
            EndDate = DateOnly.FromDateTime(end);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
