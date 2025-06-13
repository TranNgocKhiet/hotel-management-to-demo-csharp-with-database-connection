using BusinessObjects;

namespace DataAccessLayer
{
    public class CustomerDAO
    {
        public static List<Customer> GetCustomers()
        {
            var customers = new List<Customer>();
            try
            {
                using var db = new MyHotelContext();
                customers = db.Customers.ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return customers;
        }

        public static Customer GetCustomer(int id)
        {
            using var db = new MyHotelContext();
            return db.Customers.FirstOrDefault(c => c.CustomerID == id);
        }

        public static Customer GetCustomerByEmailAndPassword(string email, string password)
        {
            try
            {
                using var db = new MyHotelContext();
                Customer customer = db.Customers.FirstOrDefault(c => c.EmailAddress.Equals(email) && c.Password.Equals(password));
                if (customer != null)
                    return customer;
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public static void AddCustomer(Customer customer)
        {
            try
            {
                using var db = new MyHotelContext();
                db.Customers.Add(customer);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void UpdateCustomer(Customer customer)
        {
            try
            {
                using var db = new MyHotelContext();
                db.Entry<Customer>(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void RemoveCustomer(Customer customer)
        {
            try
            {
                using var db = new MyHotelContext();
                var c = db.Customers.SingleOrDefault(x => x.CustomerID == customer.CustomerID);
                if (c != null)
                {
                    db.Customers.Remove(c);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
