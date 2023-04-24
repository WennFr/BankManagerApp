using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.ViewModels.AccountView;

public enum AccountErrorCode
{
    OK,
    AccountNotFound
}

namespace BankRepository.Services.AccountService
{
    public interface IAccountService
    {

        decimal GetTotalCustomerAccountBalance(int customerId);

        PagedAccountViewModel GetAllAccounts(string sortColumn, string sortOrder, int pageNo);

        AccountViewModel GetAccountByAccountId(int accountId);

        List<AccountViewModel> GetAccountsByCustomerId(int customerId);

        AccountErrorCode ReturnValidationStatus(int accountId);

        void RegisterNewAccountByCustomerId(int customerId);


    }
}
