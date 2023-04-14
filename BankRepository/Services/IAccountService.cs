using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.ViewModels;

public enum AccountErrorCode
{
    OK,
    AccountNotFound
}

namespace BankRepository.Services
{
    public interface IAccountService
    {

        decimal GetTotalCustomerAccountBalance(int customerId);

        PagedAccountViewModel GetAllAccounts(string sortColumn, string sortOrder,int pageNo);

        AccountViewModel GetAccountByAccountId(int accountId);

        List<AccountViewModel> GetAccountsByCustomerId(int customerId);

        AccountErrorCode ReturnValidationStatus(int accountId);


    }
}
