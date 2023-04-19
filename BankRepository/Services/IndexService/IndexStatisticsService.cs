using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankRepository.BankAppData;
using BankRepository.ViewModels.IndexView;
using Microsoft.EntityFrameworkCore;

namespace BankRepository.Services.IndexService
{
    public class IndexStatisticsService : IIndexStatisticsService
    {
        public IndexStatisticsService(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly BankAppDataContext _dbContext;

        public IEnumerable<IndexDataViewModel> GetIndexCountryStatistics()
        {
            foreach (var country in _dbContext.Customers.Select(c => c.Country).Distinct())
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

                    Country = country
                };
            }
        }
    }
}
