﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;

namespace BankRepository.ViewModels
{
    public class TopCustomerViewModel 
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal TotalBalanceOfAllAccounts { get; set; }

    }
}