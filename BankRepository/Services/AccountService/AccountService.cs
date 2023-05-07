using BankRepository.BankAppData;
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
using System.Xml.Linq;
using AutoMapper;
using BankRepository.ViewModels.AccountView;

namespace BankRepository.Services.AccountService
{
    public class AccountService : IAccountService
    {

        public AccountService(BankAppDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private readonly BankAppDataContext _dbContext;
        private readonly IMapper _mapper;


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



            var accountViewModelResult = _mapper.Map<List<AccountViewModel>>(pagedResult.Results);

            var pagedAccountViewModelResult = new PagedAccountViewModel
            {
                Accounts = accountViewModelResult,
                PageCount = pagedResult.PageCount
            };


            return pagedAccountViewModelResult;
        }

        public AccountViewModel GetAccountByAccountId(int accountId)
        {
            var account = _dbContext.Accounts
                .Where(a => a.AccountId == accountId)
                .FirstOrDefault();

            var accountViewModelResult = _mapper.Map<AccountViewModel>(account);

            return accountViewModelResult;
        }


        public List<AccountViewModel> GetAccountsByCustomerId(int customerId)
        {

            var query = _dbContext.Dispositions.AsQueryable();

            var accounts = query
                .Include(d => d.Account)
                .ThenInclude(a => a.Dispositions)
                .ThenInclude(d => d.Customer)
                .Where(d => d.CustomerId == customerId && d.Type.ToLower() == "owner");

            var accountViewModelResult = _mapper.Map<List<AccountViewModel>>(accounts);

            return accountViewModelResult;
        }

        public AccountErrorCode ReturnValidationStatus(int accountId)
        {

            var account = _dbContext.Accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null)
            {
                return AccountErrorCode.AccountNotFound;
            }

            return AccountErrorCode.OK;
        }

        public void RegisterNewAccountByCustomerId(int customerId)
        {
            var customer = _dbContext.Customers.First(c => c.CustomerId == customerId);

            var newAccount = new Account()
            {
                Frequency = "Monthly",
                Created = DateTime.Now.Date,
                Balance = 0.00m
            };

            _dbContext.Accounts.Add(newAccount);
            _dbContext.SaveChanges();

            _dbContext.Dispositions.Add(new Disposition()
            {
                CustomerId = customer.CustomerId,
                AccountId = newAccount.AccountId,
                Type = "OWNER"
            });

            _dbContext.SaveChanges();
        }


        public string GetCurrency()
        {
            return "€";
        }

    }
}
