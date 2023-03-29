using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using BankRepository.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BankRepository.Services
{
    public class CustomerService : ICustomerService
    {

        public CustomerService(BankAppDataContext dbContext, IAccountService accountService)
        {

            _dbContext = dbContext;
            _accountService = accountService;

        }

        private readonly BankAppDataContext _dbContext;
        private readonly IAccountService _accountService;


        public List<TopCustomerViewModel> GetTopCustomersByCountry(string country)
        {
            var query = _dbContext.Dispositions
                .Include(d => d.Customer)
                .Where(d=> d.Customer.Country == country && d.Type.ToLower() == "owner")
               
                .AsQueryable();

            var result = query.Select(d => new TopCustomerViewModel()
            {
                Id = d.Customer.CustomerId,
                GivenName = d.Customer.Givenname,
                Surname = d.Customer.Surname,
                City = d.Customer.City,
                Country = d.Customer.Country,
                TotalBalanceOfAllAccounts = _accountService.GetTotalCustomerBalance(d.Customer.CustomerId)
            }).ToList();

            return result;
        }




    }
}
