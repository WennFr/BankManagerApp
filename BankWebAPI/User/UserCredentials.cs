using System.Security.Claims;
using BankRepository.Services.CustomerService;

namespace BankWebAPI.User
{
    public class UserCredentials
    {

        public UserCredentials(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        private readonly ICustomerService _customerService;


        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() // Can only Read
            {
                UserName = "richard_admin",
                EmailAddress = "richard_admin@email.se",
                Password = "passwordAdmin",
                GivenName = "Richard",
                SurName = "Chalk",
                Role = "Admin",
                Claims = new List<Claim>
                {
                    new Claim("CustomerId", "Admin")
                }
            }
        };

        
        public List<UserModel> GetUsers()
        {
            var customers = _customerService.GetAllCustomers("", "", 1, int.MaxValue, "", "").Customers;

            var users = customers.Select(c => new UserModel
            {
                UserName = $"user_{c.CustomerId}",
                EmailAddress = $"{c.Givenname.ToLower()}_{c.Surname.ToLower()}@bank.com",
                Password = c.CustomerId.ToString(),
                GivenName = c.Givenname,
                SurName = c.Surname,
                Role = "User",
                Claims = new List<Claim>
                {
                    new Claim("CustomerId", c.CustomerId.ToString())
                }

            }).ToList();

            // Add the new users to the static Users list
            Users.AddRange(users);

            return users;
        }


    }

}
