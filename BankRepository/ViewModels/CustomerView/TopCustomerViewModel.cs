using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;

namespace BankRepository.ViewModels.CustomerView
{
    public class TopCustomerViewModel
    {
        public int CustomerId { get; set; }
        public string Givenname { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal TotalBalanceOfAllAccounts { get; set; }
        public string Currency { get; set; }

    }
}
