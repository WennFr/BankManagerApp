using System.Security.Claims;

namespace BankWebAPI.User
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string SurName { get; set; }
        public string GivenName { get; set; }
        public List<Claim>? Claims { get; set; }
    }

}
