using AutoMapper;
using BankManagerApp.Pages.CustomerManagement;
using BankRepository.BankAppData;
using BankRepository.ViewModels.CustomerView;

namespace BankManagerApp.PageProfiles
{
    public class CreateProfile : Profile
    {
        public CreateProfile()
        {
            CreateMap<CreateCustomerModel, CustomerInformationViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.GenderCustomer.ToString()))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryCustomer.ToString()))
                .ForMember(dest => dest.TelephoneCountryCode, opt => opt.MapFrom(src => src.TelephoneCountryCodeCustomer.ToString()));

        }
    }
}
