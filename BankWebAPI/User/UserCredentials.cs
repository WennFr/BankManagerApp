using BankRepository.Services.CustomerService;

namespace BankWebAPI.User
{
    public class UserCredentials
    {
        private readonly ICustomerService _customerService;

        public UserCredentials(ICustomerService customerService)
        {
            _customerService = customerService;
        }



        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() // Can only Read
            {
                UserName = "richard_user",
                EmailAddress = "richard_user@email.se",
                Password = "passwordUser",
                GivenName = "Richard",
                SurName = "Chalk",
                Role = "User",
            }

        };
    }

}
