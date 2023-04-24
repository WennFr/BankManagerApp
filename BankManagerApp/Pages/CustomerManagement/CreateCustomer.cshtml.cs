using BankRepository.Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using BankRepository.Services.CustomerService;

namespace BankManagerApp.Pages.CustomerManagement
{
    [BindProperties]
    public class CreateCustomerModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public CreateCustomerModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Range(1, 99, ErrorMessage = "Please choose a valid gender!")]
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

        [StringLength(2)]
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public DateTime BirthDay { get; set; }

        public string? NationalId { get; set; }

        public string? TelephoneCountryCode { get; set; }

        public string? TelephoneNumber { get; set; }



        [Range(0, 100000, ErrorMessage = "Skriv ett tal mellan 0 och 100000")]

       

   

        [StringLength(150)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public void OnGet()
        {
            FillGenderList();
           
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
