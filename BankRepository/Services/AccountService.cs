using BankRepository.BankAppData;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BankRepository.Services
{
    public class AccountService : IAccountService
    {

        public AccountService(BankAppDataContext dbContext)
        {

            _dbContext = dbContext;

        }

        private readonly BankAppDataContext _dbContext;


        public decimal GetTotalCustomerAccountBalance(int customerId)
        {

            var TotalAccountsBalance = _dbContext.Dispositions
                .Where(d => d.CustomerId == customerId && d.Type.ToLower() == "owner")
                .Sum(d => d.Account.Balance);


            return TotalAccountsBalance;
        }
        public List<AccountViewModel> GetAllAccounts(string sortColumn, string sortOrder, int pageNo)
        {

            var query = _dbContext.Accounts.AsQueryable();

            if (sortColumn == "Frequency")
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.Frequency);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(a => a.Frequency);

            if (sortColumn == "Date Of Creation")
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.Created);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(a => a.Created);

            if (sortColumn == "Balance")
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.Balance);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(a => a.Balance);


            var firstItemIndex = (pageNo - 1) * 10;

            query = query.Skip(firstItemIndex);
            query = query.Take(10);

            var viewModelResult = query.Select(a => new AccountViewModel
            {
                AccountId = a.AccountId,
                Frequency = a.Frequency,
                DateOfCreation = a.Created.ToString(),
                Balance = a.Balance

            }).ToList();

            return viewModelResult;
        }

        public AccountViewModel GetAccountById(int accountId)
        {
            var viewModelResult = _dbContext.Accounts
                .Where(a => a.AccountId == accountId)
                .Select(a => new AccountViewModel()
                {
                    AccountId = a.AccountId,
                    Frequency = a.Frequency,
                    DateOfCreation = a.Created.ToString(),
                    Balance = a.Balance
                }).FirstOrDefault();

            return viewModelResult;



        }



    }
}
