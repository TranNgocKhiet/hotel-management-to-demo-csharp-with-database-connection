using BusinessObjects;
using Repositories;
using Services;
using System.Windows;
using System.Windows.Controls;

namespace TranNgocKhietWPF
{
    public partial class UserProfileEditPage : Page
    {
        private readonly ICustomerService iCustomerService;
        private Customer currentUser;

        public UserProfileEditPage(Customer user)
        {
            InitializeComponent();

            var customerRepository = CustomerRepository.Instance;
            var customerService = new CustomerService(customerRepository);
            iCustomerService = customerService;

            currentUser = user;

            txtFullName.Text = currentUser.CustomerFullName;
            txtTelephone.Text = currentUser.Telephone;
            txtEmailAddress.Text = currentUser.EmailAddress;

            txtCustomerBirthday.Text = currentUser.CustomerBirthday.HasValue
                ? currentUser.CustomerBirthday.Value.ToString("dd/MM/yyyy") : "";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            currentUser.CustomerFullName = txtFullName.Text;
            currentUser.Telephone = txtTelephone.Text;
            currentUser.EmailAddress = txtEmailAddress.Text;
            currentUser.CustomerBirthday = DateOnly.Parse(txtCustomerBirthday.Text);

            iCustomerService.UpdateCustomer(currentUser);

            MessageBox.Show("Profile updated successfully!");

        }
    }
}
