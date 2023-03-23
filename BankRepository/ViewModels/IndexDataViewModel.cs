using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.ViewModels
{
    public class IndexDataViewModel
    {
        public int TotalNumberOfCustomers { get; set; }
        public int TotalNumberOfAccounts { get; set; }
        public decimal TotalSumOfAccounts { get; set; }

    }
}
