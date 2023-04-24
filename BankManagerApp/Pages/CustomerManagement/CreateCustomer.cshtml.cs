using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BankManagerApp.Pages.CustomerManagement
{
    [BindProperties]
    public class CreateCustomerModel : PageModel
    {




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

        [Range(0, 100)]
        public int BirthDay { get; set; }


        public string? NationalId { get; set; }


        public string? TelephoneCountryCode { get; set; }

        public string? TelephoneNumber { get; set; }



        [Range(0, 100000, ErrorMessage = "Skriv ett tal mellan 0 och 100000")]
        public decimal Salary { get; set; }

       

   

        [StringLength(150)]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public void OnGet()
        {
        }
    }
}
