using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.ViewModels.CustomerView
{
    public class CustomerInformationViewModel
    {
        public int CustomerId { get; set; }
        public string Gender { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string? BirthDay { get; set; }
        public string? NationalId { get; set; }
        public string? TelephoneCountryCode { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public decimal TotalBalanceOfAllAccounts { get; set; }





    }
}
