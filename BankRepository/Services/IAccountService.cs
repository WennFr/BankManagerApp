using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.ViewModels;

namespace BankRepository.Services
{
    public interface IAccountService
    {

        decimal GetTotalCustomerAccountBalance(int customerId);

        List<AccountViewModel> GetAllAccounts(string sortColumn, string sortOrder,int pageNo);

        AccountViewModel GetAccountById(int accountId);

    }
}
