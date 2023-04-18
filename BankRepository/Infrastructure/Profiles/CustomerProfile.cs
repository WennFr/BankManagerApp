using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using BankRepository.ViewModels;
using BankRepository.Infrastructure.Paging;

namespace BankRepository.Infrastructure.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerViewModel>().ReverseMap();

            //CreateMap<PagedResult<Customer>, PagedResult<CustomerViewModel>>()
            //    .ReverseMap();

        }
    }
}
