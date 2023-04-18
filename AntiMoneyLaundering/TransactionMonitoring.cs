using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using BankRepository.DataAccess;
using BankRepository.Services;
using BankRepository.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace AntiMoneyLaundering
{
    public class TransactionMonitoring
    {
        public TransactionMonitoring(ICustomerService customerService, IAccountService accountService, ITransactionService transactionService)
        {
            _customerService = customerService;
            _accountService = accountService;
            _transactionService = transactionService;
        }

        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;

        private List<SuspectedTransaction> suspectedTransactionsByCountry;

        public void Execute()
        {
            suspectedTransactionsByCountry = new List<SuspectedTransaction>();
            var allCustomers = _customerService.GetAllCustomers(null, null, 1, null, null, true).Customers;
            var counter = 1;
            DateTime lastMonitoringDate = DateTime.Now;


            if (!File.Exists("../../../MonitoringData/lastTransactionMonitoring.txt"))
            {
                using (StreamWriter writer = new StreamWriter($"../../../MonitoringData/lastTransactionMonitoring.txt", append: true))
                {
                    var date = new DateTime(2010, 1, 1);
                    writer.WriteLine($"{date}");
                }
            }

            if (File.Exists("../../../MonitoringData/lastTransactionMonitoring.txt"))
            {
                using (StreamReader reader = new StreamReader("../../../MonitoringData/lastTransactionMonitoring.txt"))
                {
                    string dateString = reader.ReadLine();
                    lastMonitoringDate = DateTime.Parse(dateString);
                }
            }

            foreach (var country in allCustomers.Select(c => c.Country).Distinct())
            {

                var accountsPerCountry = allCustomers
                    .Where(c => c.Country == country)
                    .SelectMany(c => _accountService.GetAccountsByCustomerId(c.CustomerId))
                    .ToList();

                foreach (var account in accountsPerCountry)
                {

                    var customer = _customerService.GetCustomerNameByAccountId(account.AccountId);
                    var fullName = $"{customer.Givenname} {customer.Surname}";
                    var transactions = _transactionService
                        .GetAllAccountTransactions(account.AccountId, 1, 20000)
                        .Where(t=> DateTime.Parse(t.Date) > lastMonitoringDate);

                    foreach (var transaction in transactions.Where(t => t.Amount > 15000 || t.Amount < -15000))
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

                    var latestAccountTransactions = transactions
                        .Where(t => DateTime.Parse(t.Date) >= DateTime.Now.AddDays(-3))
                        .ToList();

                    var totalSumOfLatestAccountTransactions = latestAccountTransactions.Sum(t => t.Amount);

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
                        suspectedTransaction.TransactionIds.AddRange(latestAccountTransactions.Select(t => t.TransactionId));
                        suspectedTransaction.TransactionDate.AddRange(latestAccountTransactions.Select(t => t.Date));
                        suspectedTransaction.Amount.AddRange(latestAccountTransactions.Select(t => t.Amount));

                        suspectedTransactionsByCountry.Add(suspectedTransaction);
                    }

                    Console.WriteLine(counter);
                    counter++;

                }

                if (suspectedTransactionsByCountry != null)
                {
                    string fileName = $"suspected_transactions_{country}.txt";
                    string filePath = Path.Combine("MonitoringData", fileName);
                    string folderPath = Path.Combine(Environment.CurrentDirectory, "MonitoringData");
                    string fullPath = Path.Combine(folderPath, fileName);


                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }


                    using (StreamWriter writer = new StreamWriter($"../../../MonitoringData/{fileName}", append: false))
                    {
                        foreach (var transaction in suspectedTransactionsByCountry)
                        {
                            writer.WriteLine($"Customer Name: {transaction.CustomerName}");
                            writer.WriteLine($"Account ID: {transaction.AccountId}");
                            writer.WriteLine($"Transaction IDs: {string.Join(", ", transaction.TransactionIds)}");
                            writer.WriteLine($"Amount: {string.Join(", ", transaction.Amount)}");
                            writer.WriteLine($"Transaction Date: {string.Join(", ", transaction.TransactionDate)}");
                            writer.WriteLine("------------------------------------");
                        }
                    }
                }

            }




            using (StreamWriter writer = new StreamWriter($"../../../MonitoringData/lastTransactionMonitoring.txt", append: false))
            {
                var date = DateTime.Now;
                writer.WriteLine($"{date}");
            }

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}

