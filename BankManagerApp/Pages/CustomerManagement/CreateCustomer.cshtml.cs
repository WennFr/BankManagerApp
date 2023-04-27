using BankRepository.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using AutoMapper;
using BankManagerApp.DropDowns;
using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.ViewModels.CustomerView;

namespace BankManagerApp.Pages.CustomerManagement
{
    [BindProperties]
    public class CreateCustomerModel : PageModel
    {


        public CreateCustomerModel(ICustomerService customerService, IAccountService accountService, ICustomerDropDown customerDropDown, IMapper mapper)
        {
            _customerService = customerService;
            _accountService = accountService;
            _customerDropDown = customerDropDown;
            _mapper = mapper;
        }

        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        private readonly ICustomerDropDown _customerDropDown;
        private readonly IMapper _mapper;

        [Range(1, 99, ErrorMessage = "Please choose a valid gender.")]
        public Gender GenderCustomer { get; set; }
        public List<SelectListItem> Genders { get; set; }

        [MaxLength(100)]
        [Required]
        public string Givenname { get; set; }

        [MaxLength(100)]
        [Required]
        public string Surname { get; set; }

        [StringLength(100)]
        [Required]
        public string StreetAddress { get; set; }

        [StringLength(50)]
        [Required]
        public string City { get; set; }

        [StringLength(10)]
        [Required]
        public string Zipcode { get; set; }

        [Range(1, 99, ErrorMessage = "Please choose a listed country.")]
        public CountryEnum CountryCustomer { get; set; }
        public List<SelectListItem> Countries { get; set; }
        public DateTime BirthDay { get; set; } = DateTime.Today.AddYears(-18).AddDays(-1);

        [Required]
        [Range(1, 500, ErrorMessage = "Please choose a listed country code.")]
        public TelephoneCountryCode TelephoneCountryCodeCustomer { get; set; }
        public List<SelectListItem> TelephoneCountryCodes { get; set; }
        public string TelephoneNumber { get; set; }

        public string? NationalId { get; set; }


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
            Genders = _customerDropDown.FillGenderList();
            Countries = _customerDropDown.FillCountryList();
            TelephoneCountryCodes = _customerDropDown.FillCountryCodeList();

            //add solution for leap years
            var age = DateTime.Today - BirthDay;
            if (age.TotalDays < 18 * 365.25)
            {
                ModelState.AddModelError("BirthDay", "Customer must be at least 18 years old to be registered.");
            }

            if (ModelState.IsValid)
            {
                var customerViewModel = _mapper.Map<CustomerInformationViewModel>(this);
                customerViewModel.CountryCode = _customerService.GetCountryCode(customerViewModel.Country);

                var newCustomerId = _customerService.RegisterNewCustomer(customerViewModel);

                _accountService.RegisterNewAccountByCustomerId(newCustomerId);

                return RedirectToPage("Index");
            }

            return Page();
        }






    }
}
