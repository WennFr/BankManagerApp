using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.Infrastructure.Common
{
    public enum TransactionType
    {
        Credit,
        Debit
    }

    public static class OperationConstant
    {
        public const string CreditInCash = "Credit In Cash";
        public const string WithdrawalOfCash = "Withdrawal of Cash";
    }
}
