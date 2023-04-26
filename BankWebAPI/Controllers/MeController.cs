using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BankRepository.BankAppData;
using BankRepository.Services.CustomerService;

namespace BankWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        public MeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        private readonly ICustomerService _customerService;



        [HttpGet]
        [Route("{id}")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Customer>> GetOne(int id)
        {
            var customerViewModel = _customerService.GetFullCustomerInformationById(id);

            if (customerViewModel == null)
            {
                return BadRequest("Customer not found");
            }
            return Ok(customerViewModel);
        }




    }
}
