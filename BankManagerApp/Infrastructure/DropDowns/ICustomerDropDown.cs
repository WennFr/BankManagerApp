using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankManagerApp.Infrastructure.DropDowns
{
    public interface ICustomerDropDown
    {
        List<SelectListItem> FillGenderList();
        List<SelectListItem> FillCountryList();

        List<SelectListItem> FillCountryCodeList();
    }
}
