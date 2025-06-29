using BusinessObjects;
using Repositories;
using Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TranNgocKhietWPF
{
    public partial class BookingReservationListPage : Page
    {
        private readonly IBookingReservationService iBookingReservationService;
        private readonly IBookingDetailService iBookingDetailService;
        private readonly ICustomerService iCustomerService;
        private readonly IRoomInformationService iRoomInformationService;

        public class BookingDetailWithRoom
        {
            public BookingDetail BookingDetail { get; set; }
            public RoomInformation Room { get; set; }

            public int BookingReservationID => BookingDetail.BookingReservationID;
            public int RoomID => BookingDetail.RoomID;
            public string? RoomNumber => Room.RoomNumber;
            public DateOnly? StartDate => BookingDetail.StartDate;
            public DateOnly? EndDate => BookingDetail.EndDate;
            public decimal? ActualPrice => BookingDetail.ActualPrice;
        }
        public class BookingWithCustomer
        {
            public BookingReservation BookingReservation { get; set; }
            public Customer Customer { get; set; }

            public int BookingReservationID => BookingReservation.BookingReservationID;
            public DateOnly? BookingDate => BookingReservation.BookingDate;
            public decimal? TotalPrice => BookingReservation.TotalPrice;
            public string? CustomerFullName => Customer.CustomerFullName;
            public Byte? BookingStatus => BookingReservation.BookingStatus;
        }
        public BookingReservationListPage()
        {
            InitializeComponent();

            BookingReservationRepository reservationRepo = new BookingReservationRepository();
            iBookingReservationService = new BookingReservationService(reservationRepo);

            BookingDetailRepository detailRepo = new BookingDetailRepository();
            iBookingDetailService = new BookingDetailService(detailRepo);

            CustomerRepository customerRepo = new CustomerRepository();
            iCustomerService = new CustomerService(customerRepo);

            RoomInformationRepository roomRepo = new RoomInformationRepository();
            iRoomInformationService = new RoomInformationService(roomRepo);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBookingList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTextBox.Text.Trim().ToLower();

            try
            {
                var customers = iCustomerService.GetCustomers();
                var reservations = iBookingReservationService.GetBookingReservations();

                var query = reservations
                    .Join(customers,
                          r => r.CustomerID,
                          c => c.CustomerID,
                          (r, c) => new BookingWithCustomer
                          {
                              BookingReservation = r,
                              Customer = c
                          });

                if (!string.IsNullOrEmpty(keyword))
                {
                    query = query.Where(dto =>
                           dto.BookingReservationID.ToString().Contains(keyword)
                        || dto.Customer.CustomerID.ToString().Contains(keyword)
                        || (dto.CustomerFullName?.ToLower().Contains(keyword) ?? false)
                        || (dto.BookingDate?.ToString("dd/MM/yyyy").Contains(keyword) ?? false)
                        || (dto.TotalPrice?.ToString().ToLower().Contains(keyword) ?? false));
                }

                BookingDataGrid.ItemsSource = query.ToList();   
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed.\n" + ex.Message,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        public void LoadBookingList()
        {
            try
            {
                var customers = iCustomerService.GetCustomers();
                var reservations = iBookingReservationService.GetBookingReservations();
                var filtered = reservations
                    .Join(customers, 
                    r => r.CustomerID,
                    c => c.CustomerID,
                    (r, c) => new BookingWithCustomer()
                    {
                        BookingReservation = r,
                        Customer = c
                    })
                    .ToList();

                BookingDataGrid.ItemsSource = null;
                BookingDataGrid.ItemsSource = filtered;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on load list of bookings");
            }
        }

        private void BookingDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BookingDataGrid.SelectedItem is not BookingWithCustomer selectedBooking)
            {
                BookingDetailDataGrid.ItemsSource = null;
                return;
            }

            try
            {
                int id = selectedBooking.BookingReservation.BookingReservationID;

                var allDetails = iBookingDetailService.GetBookingDetails()
                                                        .Where(d => d.BookingReservationID == id)
                                                        .ToList();

                var allRooms = iRoomInformationService.GetRoomInformations(); 

                var detailWithRoomList = allDetails
                    .Join(allRooms,
                            d => d.RoomID,
                            r => r.RoomID,
                            (d, r) => new BookingDetailWithRoom
                            {
                                BookingDetail = d,
                                Room = r
                            })
                    .ToList();

                BookingDetailDataGrid.ItemsSource = detailWithRoomList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load booking details.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingDataGrid.SelectedItem is BookingWithCustomer selectedBooking)
            {
                var dialog = new BookingReservationDialog(selectedBooking.BookingReservation);
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
            var selectedBooking = BookingDataGrid.SelectedItem as BookingWithCustomer;

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

            int id = selectedBooking.BookingReservationID;

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    List<BookingDetail> bookingDetails = iBookingDetailService.GetBookingDetails()
                        .Where(d => d.BookingReservationID == selectedBooking.BookingReservationID)
                        .ToList();
                    foreach (var d in iBookingDetailService.GetBookingDetails()
                                               .Where(d => d.BookingReservationID == id)
                                               .ToList())
                    {
                        iBookingDetailService.RemoveBookingDetail(d);
                    }

                    iBookingReservationService.RemoveBookingReservation(selectedBooking.BookingReservation);
                    MessageBox.Show("Deleted.");
                    LoadBookingList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete booking.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
