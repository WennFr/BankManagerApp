using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankRepository.BankAppData;
using BankRepository.Services.AccountService;
using BankRepository.ViewModels.IndexView;
using Microsoft.EntityFrameworkCore;

namespace BankRepository.Services.IndexService
{
    public class IndexStatisticsService : IIndexStatisticsService
    {
        public IndexStatisticsService(BankAppDataContext dbContext, IAccountService accountService)
        {
            _dbContext = dbContext;
            _accountService = accountService;
        }

        private readonly BankAppDataContext _dbContext;
        private readonly IAccountService _accountService;

        public IEnumerable<IndexDataViewModel> GetIndexCountryStatistics()
        {
            foreach (var country in _dbContext.Customers.Select(c => c.Country).Distinct().ToList())
            {
                yield return new IndexDataViewModel
                {
                    TotalNumberOfCustomers = _dbContext.Customers.Where(c => c.Country == country).Count(),

                    TotalNumberOfAccounts = _dbContext.Dispositions
                        .Where(d => d.Customer.Country == country && d.Type.ToLower() == "owner")
                        .Distinct()
                        .Count(),
                    TotalSumOfAccounts = _dbContext.Dispositions
                        .Include(d => d.Account)
                        .Where(d => d.Customer.Country == country && d.Type.ToLower() == "owner")
                        .Sum(d => d.Account.Balance),

                    Country = country,
                    Currency = _accountService.GetCurrency()
                };
            }
        }
    }
}
