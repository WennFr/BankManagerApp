using BankRepository.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankManagerApp.DropDowns
{
    public class CustomerDropDown : ICustomerDropDown
    {
        public List<SelectListItem> FillGenderList()
        {
            return Enum.GetValues<Gender>()
                .Select(g => new SelectListItem
                {
                    Value = ((int)g).ToString(),
                    Text = g.ToString()
                }).ToList();
        }

        public List<SelectListItem> FillCountryList()
        {
            return Enum.GetValues<CountryEnum>()
                .Select(g => new SelectListItem
                {
                    Value = g.ToString(),
                    Text = g.ToString()
                }).ToList();
        }

        public List<SelectListItem> FillCountryCodeList()
        {
            return Enum.GetValues<TelephoneCountryCode>()
                .Select(g => new SelectListItem
                {
                    Text = ((int)g).ToString(),
                    Value = g.ToString()
                }).ToList();
        }


    }
}
