using Repositories;
using Services;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;

namespace TranNgocKhietWPF
{
    public partial class RoomInformationListPage : Page
    {
        private readonly IRoomInformationService iRoomInformationService;
        private readonly IRoomTypeService iRoomTypeService;
        public class RoomDisplayDto
        {
            public int RoomID { get; set; }
            public string? RoomNumber { get; set; }
            public string? RoomDetailDescription { get; set; }
            public int? RoomMaxCapacity { get; set; }
            public int? RoomStatus { get; set; }
            public decimal? RoomPricePerDay { get; set; }

            public string? RoomTypeName { get; set; }
        }

        public RoomInformationListPage()
        {
            InitializeComponent();

            var roomRepository = new RoomInformationRepository();
            iRoomInformationService = new RoomInformationService(roomRepository);

            var roomTypeRepository = new RoomTypeRepository();
            iRoomTypeService = new RoomTypeService(roomTypeRepository);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRoomList();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = SearchTextBox.Text.Trim().ToLower();

            try
            {
                var rooms = iRoomInformationService.GetRoomInformations();
                var roomTypes = iRoomTypeService.GetRoomTypes();

                var displayList = rooms.Select(r => new RoomDisplayDto
                {
                    RoomID = r.RoomID,
                    RoomNumber = r.RoomNumber,
                    RoomDetailDescription = r.RoomDetailDescription,
                    RoomMaxCapacity = r.RoomMaxCapacity,
                    RoomStatus = r.RoomStatus,
                    RoomPricePerDay = r.RoomPricePerDay,
                    RoomTypeName = roomTypes.FirstOrDefault(rt => rt.RoomTypeID == r.RoomTypeID)?.RoomTypeName ?? "Unknown"
                }).ToList();

                if (!string.IsNullOrEmpty(keyword))
                {
                    displayList = displayList.Where(r =>
                        (!string.IsNullOrEmpty(r.RoomNumber) && r.RoomNumber.ToLower().Contains(keyword)) ||
                        (!string.IsNullOrEmpty(r.RoomDetailDescription) && r.RoomDetailDescription.ToLower().Contains(keyword)) ||
                        (!string.IsNullOrEmpty(r.RoomTypeName) && r.RoomTypeName.ToLower().Contains(keyword))
                    ).ToList();
                }

                RoomDataGrid.ItemsSource = null;
                RoomDataGrid.ItemsSource = displayList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error while searching room information");
            }
        }


        public void LoadRoomList()
        {
            try
            {
                var rooms = iRoomInformationService.GetRoomInformations();
                var roomTypes = iRoomTypeService.GetRoomTypes();

                var displayList = rooms.Select(r => new RoomDisplayDto
                {
                    RoomID = r.RoomID,
                    RoomNumber = r.RoomNumber,
                    RoomDetailDescription = r.RoomDetailDescription,
                    RoomMaxCapacity = r.RoomMaxCapacity,
                    RoomStatus = r.RoomStatus,
                    RoomPricePerDay = r.RoomPricePerDay,
                    RoomTypeName = roomTypes.FirstOrDefault(rt => rt.RoomTypeID == r.RoomTypeID)?.RoomTypeName ?? "Unknown"
                }).ToList();

                RoomDataGrid.ItemsSource = displayList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error loading room information");
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            List<RoomType> roomTypes = RoomTypeRepository.Instance.GetRoomTypes();

            var dialog = new RoomInformationDialog(roomTypes);
            if (dialog.ShowDialog() == true)
            {
                var newRoom = dialog.RoomInfo;

                var rooms = iRoomInformationService.GetRoomInformations();

                iRoomInformationService.AddRoomInformation(newRoom);
                LoadRoomList();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomDataGrid.SelectedItem is RoomDisplayDto selectedDto)
            {
                var originalRoom = iRoomInformationService.GetRoomInformation(selectedDto.RoomID);
                if (originalRoom == null)
                {
                    MessageBox.Show("Room not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                List<RoomType> roomTypes = RoomTypeRepository.Instance.GetRoomTypes();
                var dialog = new RoomInformationDialog(roomTypes, originalRoom);
                if (dialog.ShowDialog() == true)
                {
                    iRoomInformationService.UpdateRoomInformation(dialog.RoomInfo);
                    LoadRoomList();
                }
            }
            else
            {
                MessageBox.Show("Please select a room to edit.");
            }
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (RoomDataGrid.SelectedItem is RoomDisplayDto selectedDto)
            {
                var room = iRoomInformationService.GetRoomInformation(selectedDto.RoomID);
                if (room == null)
                {
                    MessageBox.Show("Room not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = MessageBox.Show(
                    $"Are you sure you want to delete room '{room.RoomNumber}'?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (iRoomInformationService.IsRoomInTransaction(room.RoomID))
                        {
                            room.RoomStatus = 2;
                            iRoomInformationService.UpdateRoomInformation(room);
                            MessageBox.Show("Room is in use. Status changed to Deleted.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            iRoomInformationService.RemoveRoomInformation(room);
                            MessageBox.Show("Room deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        LoadRoomList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during delete operation.\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a room to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}