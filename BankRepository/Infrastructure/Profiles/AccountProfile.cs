using AutoMapper;
using BankRepository.BankAppData;
using BankRepository.ViewModels.AccountView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.Infrastructure.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<Account, AccountViewModel>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created.ToString("yyyy-MM-dd")))
                .ReverseMap();

            CreateMap<Disposition, AccountViewModel>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Account.AccountId))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Account.Frequency))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Account.Created.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Account.Balance));


        }
    }
}
