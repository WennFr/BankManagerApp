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
            CreateMap<Disposition, AccountViewModel>().ReverseMap();

        }
    }
}
