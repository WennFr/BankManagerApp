using BankRepository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using BankRepository.Infrastructure.Common;
using BankRepository.Infrastructure.Paging;

namespace BankRepository.Services
{
    public class TransactionService : ITransactionService
    {

        public TransactionService(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        private readonly BankAppDataContext _dbContext;

       public List<TransactionViewModel> GetAllAccountTransactions(int accountId, int pageNo, int pageSize)
       {

           var query = _dbContext.Transactions
               .Where(t=> t.AccountId == accountId)
               .OrderByDescending(t => t.Date)
               .ThenByDescending(t => t.TransactionId)
               .AsQueryable();
            
            var viewModelResult = query
                .GetPaged(pageNo, pageSize).Results
                .Select(t => new TransactionViewModel()
            {
                TransactionId = t.TransactionId,
                TransactionDate = t.Date.ToString("yyyy-MM-dd"),
                Type = t.Type,
                Operation = t.Operation,
                Amount = t.Amount,
                BalanceAfterTransaction = t.Balance

                }).ToList();
               

            return viewModelResult;

        }

       public decimal RegisterDeposit(int accountId, decimal amount)
       {
            var account = _dbContext.Accounts.First(a => a.AccountId == accountId);
            account.Balance += amount;

            _dbContext.SaveChanges();
            return account.Balance;
       }



       public decimal RegisterWithdrawal(int accountId, decimal amount)
       {
           var account = _dbContext.Accounts.First(a => a.AccountId == accountId);
           account.Balance -= amount;

           _dbContext.SaveChanges();
           return account.Balance;
       }

        public void RegisterTransaction(int accountId, decimal amount, decimal newBalance, string operation, DateTime transactionDate, TransactionType type)
       {
           _dbContext.Transactions.Add(new Transaction
           {
               AccountId = accountId,
               Date = transactionDate,
               Type = type.ToString(),
               Operation = operation,
               Amount = amount,
               Balance = newBalance,
               Symbol = "",
               Bank = null,
               Account = null
           });

           _dbContext.SaveChanges();

       }

        public TransactionErrorCode ReturnValidationStatus(decimal balance, decimal amount)
        {
            if (balance < amount)
            {
                return TransactionErrorCode.BalanceTooLow;
            }

            if (amount < 100 || amount > 10000)
            {
                return TransactionErrorCode.IncorrectAmount;
            }

            return TransactionErrorCode.OK;
        }

    }
}
