using System;
using System.Collections.Generic;

namespace BankManagerApp.BankAppData
{
    public partial class Account
    {
        public Account()
        {
            Dispositions = new HashSet<Disposition>();
            Loans = new HashSet<Loan>();
            PermenentOrders = new HashSet<PermenentOrder>();
            Transactions = new HashSet<Transaction>();
        }

        public int AccountId { get; set; }
        public string Frequency { get; set; } = null!;
        public DateTime Created { get; set; }
        public decimal Balance { get; set; }

        public virtual ICollection<Disposition> Dispositions { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual ICollection<PermenentOrder> PermenentOrders { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
