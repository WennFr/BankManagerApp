using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankManagerApp.DropDowns
{
    public interface ICustomerDropDown
    {
        List<SelectListItem> FillGenderList();
    }
}
