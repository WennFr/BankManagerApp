using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BankRepository.BankAppData;
using BankRepository.Services.AccountService;
using BankRepository.Services.CustomerService;
using BankRepository.ViewModels.CustomerView;
using Microsoft.AspNetCore.Cors;

namespace BankWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class CustomerController : ControllerBase
    {
        public CustomerController(ICustomerService customerService, IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
        }

        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;






        // Get ONE ///////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve Customer Information From The Database
        /// </summary>
        /// <returns>
        /// Customer Information
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/customer/2
        /// </remarks>
        /// <response code="200">
        /// Successfully Returned Customer Information
        /// </response>


        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Admin,User")]
        [Authorize(Policy = "CustomerIdPolicy")]
        public async Task<ActionResult<CustomerInformationViewModel>> GetOne(int id)
        {
            var loggedInCustomerId = User.Claims.FirstOrDefault(c => c.Type == "CustomerId")?.Value;

            if (id.ToString() != loggedInCustomerId && User.IsInRole("User"))
            {
                return Unauthorized();
            }

            var customerViewModel = _customerService.GetFullCustomerInformationById(id);

            if (customerViewModel == null)
            {
                return BadRequest("Customer not found");
            }
            return Ok(customerViewModel);
        }




    }
}
