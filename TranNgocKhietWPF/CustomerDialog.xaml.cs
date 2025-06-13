using BusinessObjects;
using Services;
using System.Windows;

namespace TranNgocKhietWPF
{
    public partial class CustomerDialog : Window
    {
        public Customer Customer { get; private set; }

        public CustomerDialog(Customer existingCustomer = null)
        {
            InitializeComponent();
            if (existingCustomer != null)
            {
                txtFullName.Text = existingCustomer.CustomerFullName;
                txtTelephone.Text = existingCustomer.Telephone;
                txtEmail.Text = existingCustomer.EmailAddress;
                txtStatus.Text = existingCustomer.CustomerStatus.ToString();
                txtPassword.Text = existingCustomer.Password;

                txtBirthday.Text = existingCustomer.CustomerBirthday.HasValue
                    ? existingCustomer.CustomerBirthday.Value.ToString("yyyy-MM-dd") : "";

                Customer = existingCustomer;
            }
            else
            {
                Customer = new Customer();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer.CustomerFullName = txtFullName.Text;
                Customer.Telephone = txtTelephone.Text;
                Customer.EmailAddress = txtEmail.Text;
                Customer.CustomerBirthday = DateOnly.Parse(txtBirthday.Text);
                Customer.CustomerStatus = Byte.Parse(txtStatus.Text);
                Customer.Password = txtPassword.Text;

                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Input error: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
