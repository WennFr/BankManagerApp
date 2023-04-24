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
                    Value = g.ToString(),
                    Text = g.ToString()
                }).ToList();
        }
    }
}
