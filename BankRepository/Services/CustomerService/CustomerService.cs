using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankRepository.BankAppData;
using BankRepository.Infrastructure.Common;
using BankRepository.Infrastructure.Paging;
using BankRepository.Services.AccountService;
using BankRepository.ViewModels.CustomerView;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BankRepository.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {

        public CustomerService(BankAppDataContext dbContext, IAccountService accountService, IMapper mapper)
        {

            _dbContext = dbContext;
            _accountService = accountService;
            _mapper = mapper;

        }

        private readonly BankAppDataContext _dbContext;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;


        public List<TopCustomerViewModel> GetTopCustomersByCountry(string country)
        {
            var query = _dbContext.Dispositions.AsQueryable();

            var customers = _dbContext.Customers
                .Where(c => c.Country.ToLower() == country.ToLower())
                .Include(c => c.Dispositions)
                .ThenInclude(d => d.Account)
                .OrderByDescending(c => c.Dispositions
                    .Where(d => d.Type.ToLower() == "owner")
                    .Sum(d => d.Account.Balance))
                .Take(10)
                .ToList();

            var viewModelResult = _mapper.Map<List<TopCustomerViewModel>>(customers);

            foreach (var customer in viewModelResult)
            {
                customer.TotalBalanceOfAllAccounts = _accountService.GetTotalCustomerAccountBalance(customer.CustomerId);
            }

            return viewModelResult;
        }

        public PagedCustomerViewModel GetAllCustomers(string sortColumn, string sortOrder, int pageNo,int pageSize ,string qName, string qCity)
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
                    query = query.OrderByDescending(c => c.City);

            if (sortColumn == "Country")
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Country);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Country);




            var pagedResult = query.GetPaged(pageNo, pageSize);

            var customerViewModelResult = _mapper.Map<List<CustomerViewModel>>(pagedResult.Results);

            var pagedCustomerViewModelResult = new PagedCustomerViewModel
            {
                Customers = customerViewModelResult,
                PageCount = pagedResult.PageCount
            };



            return pagedCustomerViewModelResult;
        }


        public CustomerInformationViewModel GetFullCustomerInformationById(int customerId)
        {

            var customer = _dbContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            var customerViewModel = _mapper.Map<CustomerInformationViewModel>(customer);
            customerViewModel.TotalBalanceOfAllAccounts = _accountService.GetTotalCustomerAccountBalance(customerViewModel.CustomerId);

            return customerViewModel;
        }

        public CustomerViewModel GetCustomerNameByAccountId(int accountId)
        {

            var result = _dbContext.Dispositions
                .Include(d => d.Account)
                .ThenInclude(a => a.Dispositions)
                .ThenInclude(d => d.Customer)
                .Where(d => d.AccountId == accountId && d.Type.ToLower() == "owner")
                .First();


            var customerViewModel = _mapper.Map<CustomerViewModel>(result.Customer);

            return customerViewModel;
        }

        public int RegisterNewCustomer(CustomerInformationViewModel customerViewModel)
        {
            var customer = new Customer();
            customer = _mapper.Map<Customer>(customerViewModel);

        

            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            return customer.CustomerId;
        }

        public void EditCustomer(CustomerInformationViewModel customerViewModel)
        {
            var customerToUpdate = _dbContext.Customers.First(c => c.CustomerId == customerViewModel.CustomerId);
            customerToUpdate = _mapper.Map<Customer>(customerViewModel);

            _dbContext.SaveChanges();

        }


        public string GetCountryCode(string country)
        {
            switch (country)
            {
                case "Sweden":
                    return "SE";
                case "Norway":
                    return "NO";
                case "Finland":
                    return "FI";
                case "Denmark":
                    return"DK";
            }
            return "";
        }

    }
}
