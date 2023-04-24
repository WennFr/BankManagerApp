using BankRepository.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using BankManagerApp.DropDowns;
using BankRepository.Services.CustomerService;

namespace BankManagerApp.Pages.CustomerManagement
{
    [BindProperties]
    public class CreateCustomerModel : PageModel
    {
       

        public CreateCustomerModel(ICustomerService customerService, ICustomerDropDown customerDropDown)
        {
            _customerService = customerService;
            _customerDropDown = customerDropDown;
        }

        private readonly ICustomerService _customerService;
        private readonly ICustomerDropDown _customerDropDown;

        [Range(1, 99, ErrorMessage = "Please choose a valid gender.")]
        public Gender GenderCustomer { get; set; }
        public List<SelectListItem> Genders { get; set; }

        public string Gender { get; set; }

        [MaxLength(100)]
        [Required]
        public string Givenname { get; set; }

        public string Surname { get; set; }

        [StringLength(100)]
        public string StreetAddress { get; set; }

        [StringLength(50)]
        [Required]
        public string City { get; set; }

        [StringLength(10)]
        public string Zipcode { get; set; }

        [Range(1, 99, ErrorMessage = "Please choose a listed country.")]
        public CountryEnum CountryCustomer { get; set; }
        public List<SelectListItem> Countries { get; set; }

        public string CountryCode { get; set; }

        public DateTime BirthDay { get; set; }

        public string? NationalId { get; set; }

        [Range(1, 500, ErrorMessage = "Please choose a listed country code.")]
        public TelephoneCountryCode TelephoneCountryCodeCustomer { get; set; }
        public List<SelectListItem> TelephoneCountryCodes { get; set; }

        public string? TelephoneNumber { get; set; }



        [StringLength(150)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public void OnGet()
        {
            Genders = _customerDropDown.FillGenderList();
            Countries = _customerDropDown.FillCountryList();
            TelephoneCountryCodes = _customerDropDown.FillCountryCodeList();


        }

        public IActionResult OnPost()
        {

            if (BirthDay < DateTime.Now)
            {
                ModelState.AddModelError("DepositDate", "Please select a current date.");
            }

            if (ModelState.IsValid)
            {
                return RedirectToPage("Index");
            }

            return Page();
        }

  




    }
}
