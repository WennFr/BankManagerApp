using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        private List<SuspectedTransaction> suspectedTransactionsByCountry;


        public void Execute()
        {
            var allCustomers = _customerService.GetAllCustomers(null, null, 1, 2000, null, null).Customers;

            var lastMonitoringDate = _fileService.SetupLastMonitoringDate();

            var monitoringDate = DateTime.Now;

            foreach (var country in allCustomers.Select(c => c.Country).Distinct())
            {
                var suspectedTransactionsByCountry = GetSuspectedTransactionsByCountry(country, lastMonitoringDate, allCustomers);

                _fileService.RegisterSuspectedTransactions(suspectedTransactionsByCountry, country, monitoringDate);
            }

            _fileService.CreateNewLastMonitoringDate();

            Console.WriteLine("Done...");
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
                var transactions = GetTransactionsForAccount(account.AccountId, lastMonitoringDate);




                foreach (var transaction in transactions.Where(x => x.Amount > 15000 || x.Amount < -15000))
                {
                    var suspectedTransaction = new SuspectedTransaction
                    {
                        CustomerName = fullName,
                        AccountId = account.AccountId,
                        TransactionDate = new List<string> { transaction.Date },
                        TransactionIds = new List<int> { transaction.TransactionId },
                        Amount = new List<decimal> { transaction.Amount },
                    };
                    suspectedTransactionsByCountry.Add(suspectedTransaction);
                }



                //suspectedTransactionsByCountry.AddRange(largeAmountTransactions);



                var totalSumOfLatestAccountTransactions = GetTotalSumOfLatestAccountTransactions(transactions);

                if (totalSumOfLatestAccountTransactions > 23000)
                {

                    var suspectedTransaction = new SuspectedTransaction
                    {
                        CustomerName = fullName,
                        AccountId = account.AccountId,
                        TransactionDate = new List<string>(),
                        TransactionIds = new List<int>(),
                        Amount = new List<decimal>()
                    };

                    suspectedTransaction.TransactionIds.AddRange(transactions.Select(t => t.TransactionId));
                    suspectedTransaction.TransactionDate.AddRange(transactions.Select(t => t.Date));
                    suspectedTransaction.Amount.AddRange(transactions.Select(t => t.Amount));


                    suspectedTransactionsByCountry.Add(suspectedTransaction);

                }

                Console.WriteLine(counter);
                counter++;
            }



            return suspectedTransactionsByCountry;
        }


        private List<TransactionViewModel> GetTransactionsForAccount(int accountId, DateTime lastMonitoringDate)
        {
            return _transactionService.GetAllAccountTransactions(accountId, 1, 20000, 0)
                .Where(t => DateTime.Parse(t.Date) > lastMonitoringDate)
                .ToList();
        }

        private List<SuspectedTransaction> GetSuspectedLargeTransactions(List<TransactionViewModel> transactions, string fullName, int accountId)
        {
            return transactions
                .Where(t => t.Amount > 15000 || t.Amount < -15000)
                .Select(t => new SuspectedTransaction
                {
                    CustomerName = fullName,
                    AccountId = accountId,
                    TransactionDate = new List<string> { t.Date },
                    TransactionIds = new List<int> { t.TransactionId },
                    Amount = new List<decimal> { t.Amount },
                })
                .ToList();
        }


        private decimal GetTotalSumOfLatestAccountTransactions(List<TransactionViewModel> transactions)
        {
            var latestAccountTransactions = transactions.Where(t => DateTime.Parse(t.Date) >= DateTime.Now.AddDays(-3)).ToList();
            return latestAccountTransactions.Sum(t => t.Amount);
        }


    }
}
