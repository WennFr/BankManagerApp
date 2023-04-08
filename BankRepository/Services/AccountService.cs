using BankRepository.BankAppData;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using BankRepository.Infrastructure.Paging;

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
        public PagedAccountViewModel GetAllAccounts(string sortColumn, string sortOrder, int pageNo)
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

            var pagedResult = query.GetPaged(pageNo, 50);

            var accountViewModelResult = pagedResult.Results.Select(a => new AccountViewModel
            {
                AccountId = a.AccountId,
                Frequency = a.Frequency,
                DateOfCreation = a.Created.ToString(),
                Balance = a.Balance

            }).ToList();


            var pagedAccountViewModelResult = new PagedAccountViewModel
            {
                Accounts = accountViewModelResult,
                PageCount = pagedResult.PageCount
            };


            return pagedAccountViewModelResult;
        }

        public AccountViewModel GetAccountByAccountId(int accountId)
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


        public List<AccountViewModel> GetAccountsByCustomerId(int customerId)
        {

            var query = _dbContext.Dispositions.AsQueryable();

            var viewModelResult = query
                .Include(d => d.Account)
                .ThenInclude(a => a.Dispositions)
                .ThenInclude(d => d.Customer)
                .Where(d => d.CustomerId == customerId && d.Type.ToLower() == "owner")
                .Select(d => new AccountViewModel
                {
                    AccountId = d.Account.AccountId,
                    Frequency = d.Account.Frequency,
                    DateOfCreation = d.Account.Created.ToString(),
                    Balance = d.Account.Balance

                }).ToList();

            return viewModelResult;


        }



    }
}
