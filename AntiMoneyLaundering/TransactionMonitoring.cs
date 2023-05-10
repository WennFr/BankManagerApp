using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AntiMoneyLaundering.Services;
using BankRepository.BankAppData;
using BankRepository.Services;
using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.Services.TransactionService;
using BankRepository.ViewModels;
using BankRepository.ViewModels.CustomerView;
using BankRepository.ViewModels.TransactionView;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AntiMoneyLaundering
{
    public class TransactionMonitoring
    {
        public TransactionMonitoring(ICustomerService customerService, IAccountService accountService, ITransactionService transactionService, IFileService fileService)
        {
            _customerService = customerService;
            _accountService = accountService;
            _transactionService = transactionService;
            _fileService = fileService;
        }

        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly IFileService _fileService;



        public void Execute()
        {

            var suspectedTransactionsByCountryToRegister = new List<SuspectedTransaction>();

            var lastMonitoringDate = _fileService.SetupLastMonitoringDate();
            var monitoringDate = DateTime.Now;

            var allCustomers = _customerService.GetAllCustomers(null, null, 1, 2000000, null, null).Customers;

            foreach (var country in allCustomers.Select(c => c.Country).Distinct())
            {
                suspectedTransactionsByCountryToRegister = GetSuspectedTransactionsByCountry(country, lastMonitoringDate, allCustomers);

                _fileService.RegisterSuspectedTransactions(suspectedTransactionsByCountryToRegister, country, monitoringDate);
            }

            _fileService.CreateNewLastMonitoringDate();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully completed transaction monitoring");
            Console.ResetColor();
            Console.ReadKey();
        }



        private List<SuspectedTransaction> GetSuspectedTransactionsByCountry(string country, DateTime lastMonitoringDate, List<CustomerViewModel> allCustomers)
        {
            var counter = 1;

            var suspectedTransactionsByCountry = new List<SuspectedTransaction>();

            var accountsPerCountry = allCustomers
                .Where(c => c.Country == country)
                .SelectMany(c => _accountService.GetAccountsByCustomerId(c.CustomerId))
                .ToList();


            foreach (var account in accountsPerCountry)
            {
                var customer = _customerService.GetCustomerNameByAccountId(account.AccountId);
                var fullName = $"{customer.Givenname} {customer.Surname}";
                var transactions = _transactionService.GetAllAccountTransactions(account.AccountId, 1, 20000, 0).ToList();


                suspectedTransactionsByCountry = CheckSuspectedLargeTransactions(transactions, fullName,
                    account.AccountId, lastMonitoringDate, suspectedTransactionsByCountry);

                suspectedTransactionsByCountry = CheckSuspectedLatestTransactions(transactions,
                    suspectedTransactionsByCountry, fullName, account.AccountId);
                
                Console.WriteLine(counter);
                counter++;
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Accounts from {country} monitored.");
            Console.ResetColor();
            return suspectedTransactionsByCountry;
        }


        private List<SuspectedTransaction> CheckSuspectedLargeTransactions(List<TransactionViewModel> transactions, string fullName, int accountId, DateTime lastMonitoringDate, List<SuspectedTransaction> suspectedTransactionsByCountry)
        {

            foreach (var transaction in transactions.Where(t => t.Amount > 15000 && DateTime.Parse(t.Date) > lastMonitoringDate || t.Amount < -15000 && DateTime.Parse(t.Date) > lastMonitoringDate))
            {
                var suspectedTransaction = new SuspectedTransaction
                {
                    CustomerName = fullName,
                    AccountId = accountId,
                    TransactionDate = new List<string> { transaction.Date },
                    TransactionIds = new List<int> { transaction.TransactionId },
                    Amount = new List<decimal> { transaction.Amount },
                };
                suspectedTransactionsByCountry.Add(suspectedTransaction);
            }

            return suspectedTransactionsByCountry;
        }

        private List<SuspectedTransaction> CheckSuspectedLatestTransactions(List<TransactionViewModel> transactions,List<SuspectedTransaction> suspectedTransactionsByCountry, string fullName, int accountId)
        {

            var latestAccountTransactions = transactions.Where(t => DateTime.Parse(t.Date) >= DateTime.Now.AddDays(-3)).ToList();

            if (latestAccountTransactions.Sum(t => t.Amount) > 23000)
            {

                var suspectedTransaction = new SuspectedTransaction
                {
                    CustomerName = fullName,
                    AccountId = accountId,
                    TransactionDate = new List<string>(),
                    TransactionIds = new List<int>(),
                    Amount = new List<decimal>()
                };

                suspectedTransaction.TransactionIds.AddRange(latestAccountTransactions.Select(t => t.TransactionId));
                suspectedTransaction.TransactionDate.AddRange(latestAccountTransactions.Select(t => t.Date));
                suspectedTransaction.Amount.AddRange(latestAccountTransactions.Select(t => t.Amount));


                suspectedTransactionsByCountry.Add(suspectedTransaction);

            }

            return suspectedTransactionsByCountry;
        }


    }
}
