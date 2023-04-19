using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BankRepository.BankAppData;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using BankRepository.Infrastructure.Common;
using BankRepository.Infrastructure.Paging;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BankRepository.ViewModels.TransactionView;

namespace BankRepository.Services.TransactionService
{
    public class TransactionService : ITransactionService
    {

        public TransactionService(BankAppDataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        private readonly BankAppDataContext _dbContext;
        private readonly IMapper _mapper;

        public List<TransactionViewModel> GetAllAccountTransactions(int accountId, int pageNo, int pageSize)
        {

            var query = _dbContext.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.TransactionId)
                .AsQueryable();

            var pagedResult = query.GetPaged(pageNo, pageSize);

            var transactionViewModelResult = _mapper.Map<List<TransactionViewModel>>(pagedResult.Results);

            return transactionViewModelResult;

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
