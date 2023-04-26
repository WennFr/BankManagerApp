using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using BankRepository.BankAppData;
using BankRepository.Services.AccountService;
using BankRepository.Services.TransactionService;

namespace BankWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        public AccountController(IAccountService accountService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _transactionService = transactionService;
        }

        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;





        // GET ALL ///////////////////////////////////////////////////////
        /// <summary>
        /// Retrieve Account Transactions By Limit And Offset
        /// </summary>
        /// <returns>
        /// Account Transactions
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/Account/2/20/10 
        /// </remarks>
        /// <response code="200">
        /// Successfully Returned Account Transactions
        /// </response>

        [HttpGet]
        [Route("{id}/{limit}/{offset}")]
        //[Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Transaction>> GetALL(int id, int limit, int offset)
        {

            var account = _accountService.GetAccountByAccountId(id);

            if (account == null)
            {
                return BadRequest("Account not found");
            }

            var transactionViewModel = _transactionService.GetAllAccountTransactions(id, 1, limit, offset);

            return Ok(transactionViewModel);
        }


    }
}
