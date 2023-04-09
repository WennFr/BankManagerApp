using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using BankRepository.Infrastructure.Paging;
using BankRepository.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

            var result = _dbContext.Customers
                .Where(c => c.Country.ToLower() == country.ToLower())
                .Select(c => new TopCustomerViewModel()
                {
                    Id = c.CustomerId,
                    GivenName = c.Givenname,
                    Surname = c.Surname,
                    City = c.City,
                    Country = c.Country,
                    TotalBalanceOfAllAccounts = _dbContext.Dispositions
                        .Where(d => d.CustomerId == c.CustomerId && d.Type.ToLower() == "owner")
                        .Sum(d => d.Account.Balance)
                })
                .OrderByDescending(c => c.TotalBalanceOfAllAccounts)
                .Take(10)
                .ToList();

            return result;
        }

        public PagedCustomerViewModel GetAllCustomers(string sortColumn, string sortOrder, int pageNo, string qName, string qCity)
        {

            var query = _dbContext.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(qName))
            {
                query = query
                    .Where(c => c.Givenname.Contains(qName) ||
                                c.Surname.Contains(qName));
            }

            if (!string.IsNullOrEmpty(qCity))
            {
                query = query
                    .Where(c => c.City.Contains(qCity));
            }





            if (sortColumn == "First Name")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Givenname);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Givenname);

            if (sortColumn == "Last Name")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Surname);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Surname);

            if (sortColumn == "Address")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Streetaddress);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Streetaddress);

            if (sortColumn == "City")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.City);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c =>c.City);

            if (sortColumn == "Country")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Country);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Country);
            

            var pagedResult = query.GetPaged(pageNo, 50);

            var customerViewModelResult = pagedResult.Results.Select(c => new CustomerViewModel
            {
                Id = c.CustomerId,
                GivenName = c.Givenname,
                Surname = c.Surname,
                Address = c.Streetaddress,
                City = c.City,
                Country = c.Country,
                
            }).ToList();

            var pagedCustomerViewModelResult = new PagedCustomerViewModel
            {
                Customers = customerViewModelResult,
                PageCount = pagedResult.PageCount
            };



            return pagedCustomerViewModelResult;
        }


        public CustomerInformationViewModel GetFullCustomerInformationById(int customerId)
        {

           var viewModelResult = _dbContext.Customers
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerInformationViewModel
                {
                    Id = c.CustomerId,
                    Gender = c.Gender,
                    GivenName = c.Givenname,
                    Surname = c.Surname,
                    Address = c.Streetaddress,
                    City = c.City,
                    Zipcode = c.Zipcode,
                    Country = c.Country,
                    CountryCode = c.CountryCode,
                    BirthDay = c.Birthday.ToString(),
                    NationalId = c.NationalId,
                    TelephoneCountryCode = c.Telephonecountrycode,
                    TelephoneNumber = c.Telephonenumber,
                    EmailAddress = c.Emailaddress,
                    TotalBalanceOfAllAccounts = _accountService.GetTotalCustomerAccountBalance(customerId)

                })
                .FirstOrDefault();

            return viewModelResult;
        }

        public CustomerViewModel GetCustomerNameByAccountId(int accountId)
        {

            var viewModelResult = _dbContext.Dispositions
                .Include(d => d.Account)
                .ThenInclude(a => a.Dispositions)
                .ThenInclude(d => d.Customer)
                .Where(d => d.AccountId == accountId && d.Type.ToLower() == "owner")
                .Select(d => new CustomerViewModel
                {
                    Id = d.Customer.CustomerId,
                    GivenName = d.Customer.Givenname,
                    Surname = d.Customer.Surname,

                })
                .FirstOrDefault();

            return viewModelResult;


        }


    }
}
