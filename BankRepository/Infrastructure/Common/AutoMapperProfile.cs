using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using BankRepository.ViewModels;

namespace BankRepository.Infrastructure.Common
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Källa => Mål
            // CreateEmployeeViewModel => Employee
            CreateMap<CustomerViewModel, Customer>()
                .ReverseMap();
        }
    }

}
