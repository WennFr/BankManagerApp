using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.ViewModels
{
    public class AccountViewModel
    {
            public int AccountId { get; set; }

            public string Frequency { get; set; }

            public string Created { get; set; }
            
            public decimal Balance { get; set; }



    }
}
