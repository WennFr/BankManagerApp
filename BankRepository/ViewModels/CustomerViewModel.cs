using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;

namespace BankRepository.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<Account> Accounts { get; set; }
        public decimal AccountBalance { get; set; }

    }
}
