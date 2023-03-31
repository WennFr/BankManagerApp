using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.ViewModels
{
    internal class TransactionViewModel
    {
        public int TransactionId { get; set; }

        public string TransactionDate { get; set; }

        public string Type { get; set; }

        public string? Operation { get; set; }

        public decimal Amount { get; set; }

        public decimal BalanceAfterTransaction { get; set; }
    }
}
