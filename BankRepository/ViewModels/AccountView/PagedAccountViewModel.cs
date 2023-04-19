using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;

namespace BankRepository.ViewModels.AccountView
{
    public class PagedAccountViewModel
    {
        public List<AccountViewModel> Accounts { get; set; }
        public int PageCount { get; set; }

    }
}
