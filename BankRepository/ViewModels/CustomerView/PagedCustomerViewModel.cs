﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.ViewModels.CustomerView
{
    public class PagedCustomerViewModel
    {
        public List<CustomerViewModel> Customers { get; set; }
        public int PageCount { get; set; }




    }
}
