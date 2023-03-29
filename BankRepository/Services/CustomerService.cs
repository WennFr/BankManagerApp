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
            var query = _dbContext.Dispositions.AsQueryable();

            var result = query
                .Include(d => d.Customer)
                .Where(d => d.Customer.Country.ToLower() == country.ToLower() && d.Type.ToLower() == "owner")
                .ToList()
                .Select(d => new TopCustomerViewModel()
                {
                    Id = d.Customer.CustomerId,
                    GivenName = d.Customer.Givenname,
                    Surname = d.Customer.Surname,
                    City = d.Customer.City,
                    Country = d.Customer.Country,
                    TotalBalanceOfAllAccounts = _accountService.GetTotalCustomerBalance(d.CustomerId)
                }).OrderByDescending(c => c.TotalBalanceOfAllAccounts).Take(10).ToList();



            return result;
        }

        public List<CustomerViewModel> GetAllCustomers(string sortColumn, string sortOrder)
        {

            var query = _dbContext.Customers.AsQueryable();

            var viewModelResult = query.Select(c => new CustomerViewModel
            {
                Id = c.CustomerId,
                GivenName = c.Givenname,
                Surname = c.Surname,
                Address = c.Streetaddress,
                City = c.City,
                Country = c.Country,
                
            }).ToList();

            return viewModelResult;
        }


    }
}
