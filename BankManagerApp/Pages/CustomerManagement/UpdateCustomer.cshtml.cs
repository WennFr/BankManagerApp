using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BankManagerApp.DropDowns;
using BankRepository.BankAppData;
using BankRepository.Infrastructure.Common;
using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.ViewModels.AccountView;
using BankRepository.ViewModels.CustomerView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankManagerApp.Pages.CustomerManagement
{
    [BindProperties]
    public class UpdateCustomerModel : PageModel
    {

        public UpdateCustomerModel(ICustomerService customerService, IAccountService accountService, ICustomerDropDown customerDropDown, IMapper mapper)
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


        private int CustomerId { get; set; }

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




        public CustomerInformationViewModel Customer { get; set; }
        public List<AccountViewModel> Accounts { get; set; }


        public void OnGet(int customerId)
        {
            Genders = _customerDropDown.FillGenderList();
            Countries = _customerDropDown.FillCountryList();
            TelephoneCountryCodes = _customerDropDown.FillCountryCodeList();

            Customer = _customerService.GetFullCustomerInformationById(customerId);
            Accounts = _accountService.GetAccountsByCustomerId(customerId).ToList();

            _mapper.Map(Customer, this);

        }

        public IActionResult OnPost(int customerId)
        {
            Genders = _customerDropDown.FillGenderList();
            Countries = _customerDropDown.FillCountryList();
            TelephoneCountryCodes = _customerDropDown.FillCountryCodeList();



            var age = DateTime.Today - BirthDay;
            if (age.TotalDays < 18 * 365.25)
            {
                ModelState.AddModelError("BirthDay", "Customer must be at least 18 years old to be registered.");
            }

            if (ModelState.IsValid)
            {
                var customerViewModel = _mapper.Map<CustomerInformationViewModel>(this);
                _customerService.EditCustomer(customerViewModel);

                return RedirectToPage("Index");
            }


            return Page();
        }


    }
}
