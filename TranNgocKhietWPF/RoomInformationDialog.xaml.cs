using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Windows;

namespace TranNgocKhietWPF
{
    public partial class RoomInformationDialog : Window
    {
        public RoomInformation RoomInfo { get; private set; }

        public RoomInformationDialog(List<RoomType> roomTypes, RoomInformation room = null)
        {
            InitializeComponent();

            cboRoomType.ItemsSource = roomTypes;

            if (room != null)
            {
                RoomInfo = room;
                txtRoomNumber.Text = room.RoomNumber;
                txtDescription.Text = room.RoomDetailDescription;
                txtCapacity.Text = room.RoomMaxCapacity.ToString();
                txtStatus.Text = room.RoomStatus.ToString();
                txtPrice.Text = room.RoomPricePerDay.ToString();
                cboRoomType.SelectedValue = room.RoomTypeID;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RoomInfo ??= new RoomInformation();

                RoomInfo.RoomNumber = txtRoomNumber.Text;
                RoomInfo.RoomDetailDescription = txtDescription.Text;
                RoomInfo.RoomMaxCapacity = int.Parse(txtCapacity.Text);
                RoomInfo.RoomStatus = byte.Parse(txtStatus.Text);
                RoomInfo.RoomPricePerDay = decimal.Parse(txtPrice.Text);
                RoomInfo.RoomTypeID = (int)cboRoomType.SelectedValue;

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid input: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
