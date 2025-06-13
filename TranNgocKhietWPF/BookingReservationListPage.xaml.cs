using Repositories;
using Services;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using System.Linq;
using System;

namespace TranNgocKhietWPF
{
    public partial class BookingReservationListPage : Page
    {
        private readonly IBookingReservationService iBookingReservationService;
        private readonly IBookingDetailService iBookingDetailService;

        public BookingReservationListPage()
        {
            InitializeComponent();

            var repo = new BookingReservationRepository();
            var service = new BookingReservationService(repo);
            iBookingReservationService = service;

            var detailRepo = new BookingDetailRepository();
            iBookingDetailService = new BookingDetailService(detailRepo);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBookingList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTextBox.Text.ToLower().Trim();

            try
            {
                var allBookings = iBookingReservationService.GetBookingReservations();
                var filtered = allBookings
                    .Where(b => b.BookingReservationID.ToString().Contains(keyword) ||
                                b.CustomerID.ToString().Contains(keyword))
                    .ToList();

                BookingDataGrid.ItemsSource = null;
                BookingDataGrid.ItemsSource = filtered;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadBookingList()
        {
            try
            {
                var bookings = iBookingReservationService.GetBookingReservations();
                BookingDataGrid.ItemsSource = null;
                BookingDataGrid.ItemsSource = bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on load list of bookings");
            }
        }

        private void BookingDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingDataGrid.SelectedItem is BookingReservation selectedBooking)
            {
                try
                {
                    var details = iBookingDetailService.GetBookingDetails()
                        .Where(d => d.BookingReservationID == selectedBooking.BookingReservationID)
                        .ToList();

                    BookingDetailDataGrid.ItemsSource = null;
                    BookingDetailDataGrid.ItemsSource = details;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load booking details.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BookingReservationDialog();
            if (dialog.ShowDialog() == true)
            {
                BookingReservation newBooking = dialog.BookingReservation;
                iBookingReservationService.AddBookingReservation(newBooking);
                LoadBookingList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingDataGrid.SelectedItem is BookingReservation selectedBooking)
            {
                var dialog = new BookingReservationDialog(selectedBooking);
                if (dialog.ShowDialog() == true)
                {
                    iBookingReservationService.UpdateBookingReservation(dialog.BookingReservation);
                    LoadBookingList();
                }
            }
            else
            {
                MessageBox.Show("Please select a booking to update!");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBooking = BookingDataGrid.SelectedItem as BookingReservation;

            if (selectedBooking == null)
            {
                MessageBox.Show("Please select a booking to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete booking ID: {selectedBooking.BookingReservationID}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    iBookingReservationService.RemoveBookingReservation(selectedBooking);
                    MessageBox.Show("Booking deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadBookingList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete booking.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CreateDetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingDataGrid.SelectedItem is BookingReservation selectedBooking)
            {
                var dialog = new BookingDetailDialog(selectedBooking.BookingReservationID);
                if (dialog.ShowDialog() == true)
                {
                    var newDetail = dialog.BookingDetail;
                    iBookingDetailService.AddBookingDetail(newDetail);
                    BookingDataGrid_SelectionChanged(null, null);
                }
            }
            else
            {
                MessageBox.Show("Please select a booking to add detail to.");
            }
        }


        private void EditDetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingDetailDataGrid.SelectedItem is BookingDetail selectedDetail)
            {
                var dialog = new BookingDetailDialog(selectedDetail);
                if (dialog.ShowDialog() == true)
                {
                    iBookingDetailService.UpdateBookingDetail(dialog.BookingDetail);
                    BookingDataGrid_SelectionChanged(null, null);
                }
            }
            else
            {
                MessageBox.Show("Please select a booking detail to update.");
            }
        }

        private void DeleteDetailButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDetail = BookingDetailDataGrid.SelectedItem as BookingDetail;

            if (selectedDetail == null)
            {
                MessageBox.Show("Please select a booking detail to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete detail for Room ID: {selectedDetail.RoomID}?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    iBookingDetailService.RemoveBookingDetail(selectedDetail);
                    MessageBox.Show("Detail deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    BookingDataGrid_SelectionChanged(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete detail.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
