using BankRepository.BankAppData;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.ViewModels.TransactionView;

namespace BankRepository.Infrastructure.Profiles
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {

            CreateMap<Transaction, TransactionViewModel>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("yyyy-MM-dd")))
                .ReverseMap();
        }

    }
}
