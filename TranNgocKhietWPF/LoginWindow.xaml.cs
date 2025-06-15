using BusinessObjects;
using Repositories;
using Services;
using System.Text.Json;
using System.Windows;
using System.IO;

namespace TranNgocKhietWPF
{
    public partial class LoginWindow : Window
    {
        private readonly ICustomerService iCustomerService;

        public class AppSettings
        {
            public EmailCredentials EmailCredentials { get; set; }
            public Dictionary<string, string> ConnectionStrings { get; set; }
        }

        public class EmailCredentials
        {
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }

        public LoginWindow()
        {
            InitializeComponent();
            var customerRepository = CustomerRepository.Instance;
            var customerService = new CustomerService(customerRepository);
            iCustomerService = customerService;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string jsonString = File.ReadAllText("appsettings.json");
            AppSettings settings = JsonSerializer.Deserialize<AppSettings>(jsonString);

            string inputEmail = txtEmail.Text;
            string inputPassword = txtPass.Password;

            if (settings?.EmailCredentials != null &&
                settings.EmailCredentials.EmailAddress == inputEmail &&
                settings.EmailCredentials.Password == inputPassword)
            {
                this.Hide();
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                return;
            }

            Customer customer = iCustomerService.Login(inputEmail, inputPassword);
            if (customer != null)
            {
                this.Hide();
                UserWindow userWindow = new UserWindow(customer);
                userWindow.Show();
            }
            else
            {
                MessageBox.Show("You are not permission !");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
