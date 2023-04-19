using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using BankRepository.Infrastructure.Paging;
using BankRepository.ViewModels.CustomerView;

namespace BankRepository.Infrastructure.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerViewModel>().ReverseMap();

            CreateMap<Customer, TopCustomerViewModel>();

            CreateMap<Customer, CustomerInformationViewModel>().ReverseMap();

            CreateMap<Disposition, CustomerViewModel>().ReverseMap();


        }
    }
}
