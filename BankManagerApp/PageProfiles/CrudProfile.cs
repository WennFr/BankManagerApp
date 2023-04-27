using AutoMapper;
using BankManagerApp.Pages.CustomerManagement;
using BankRepository.BankAppData;
using BankRepository.Infrastructure.Common;
using BankRepository.ViewModels.CustomerView;

namespace BankManagerApp.PageProfiles
{
    public class CrudProfile : Profile
    {
        public CrudProfile()
        {
            CreateMap<CreateCustomerModel, CustomerInformationViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderCustomer.ToString()))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryCustomer.ToString()))
                .ForMember(dest => dest.TelephoneCountryCode, opt => opt.MapFrom(src => src.TelephoneCountryCodeCustomer.ToString()));


            CreateMap<CustomerInformationViewModel, UpdateCustomerModel>()
                .ForMember(dest => dest.GenderCustomer, opt => opt.MapFrom(src => Enum.Parse(typeof(Gender), src.Gender)))
                .ForMember(dest => dest.CountryCustomer, opt => opt.MapFrom(src => Enum.Parse(typeof(CountryEnum), src.Country)))
                .ForMember(dest => dest.TelephoneCountryCodeCustomer, opt => opt.MapFrom(src => Enum.Parse(typeof(TelephoneCountryCode), src.TelephoneCountryCode)))
                .ReverseMap()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderCustomer.ToString()))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryCustomer.ToString()))
                .ForMember(dest => dest.TelephoneCountryCode, opt => opt.MapFrom(src => src.TelephoneCountryCodeCustomer.ToString()));


        }
    }
}
