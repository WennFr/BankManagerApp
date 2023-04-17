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

            foreach (var country in allCustomers.Select(c => c.Country).Distinct())
            {
                var accountsPerCountry = allCustomers
                    .Where(c => c.Country == country)
                    .SelectMany(c => _accountService.GetAccountsByCustomerId(c.Id))
                    .ToList();

                foreach (var account in accountsPerCountry)
                {

                    var customer = _customerService.GetCustomerNameByAccountId(account.AccountId);
                    var fullName = $"{customer.GivenName} {customer.Surname}";


                    var transactions = _transactionService.GetAllAccountTransactions(account.AccountId, 1, 20000);

                    foreach (var transaction in transactions.Where(t => t.Amount > 15000 || t.Amount < -15000))
                    {
                        var suspectedTransaction = new SuspectedTransaction
                        {
                            CustomerName = fullName,
                            AccountId = account.AccountId,
                            TransactionDate = transaction.TransactionDate,
                            TransactionIds = new List<int> { transaction.TransactionId },
                            Amount = transaction.Amount,
                        };
                        suspectedTransactionsByCountry.Add(suspectedTransaction);
                    }

                    var latestAccountTransactions = transactions
                        .Where(t => DateTime.Parse(t.TransactionDate) >= DateTime.Now.AddDays(-3))
                        .ToList();

                    var totalSumOfLatestAccountTransactions = latestAccountTransactions.Sum(t => t.Amount);

                    if (totalSumOfLatestAccountTransactions > 23000)
                    {
                        var suspectedTransaction = new SuspectedTransaction
                        {
                            CustomerName = fullName,
                            AccountId = account.AccountId,
                            TransactionIds = new List<int>(),

                        };
                        suspectedTransaction.TransactionIds.AddRange(latestAccountTransactions.Select(t => t.TransactionId));

                        suspectedTransactionsByCountry.Add(suspectedTransaction);
                    }

                    Console.WriteLine(counter);
                    counter++;

                }

                if (suspectedTransactionsByCountry != null)
                {
                    string fileName = "suspected_transactions.txt";
                    string filePath = Path.Combine(Environment.CurrentDirectory, fileName);
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        foreach (var transaction in suspectedTransactionsByCountry)
                        {
                            writer.WriteLine($"Customer Name: {transaction.CustomerName}");
                            writer.WriteLine($"Account ID: {transaction.AccountId}");
                            writer.WriteLine($"Transaction IDs: {string.Join(", ", transaction.TransactionIds)}");
                            writer.WriteLine($"Amount: {transaction.Amount}");
                            writer.WriteLine($"Transaction Date: {transaction.TransactionDate}");
                            writer.WriteLine("----------------------------");
                        }
                    }
                }

            }

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}

