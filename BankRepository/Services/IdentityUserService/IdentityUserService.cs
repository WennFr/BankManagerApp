using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankRepository.BankAppData;
using BankRepository.ViewModels.IdentityUserView;
using Microsoft.AspNetCore.Identity;

namespace BankRepository.Services.IdentityUserService
{
    public class IdentityUserService : IIdentityUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public IdentityUserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        
        public void SaveNewIdentityUser(IdentityUserViewModel identityUserViewModel)
        {

        }

        public List<IdentityUserViewModel> GetAllIdentityUsers()
        {
            var query = _userManager.Users;

           var identityUserViewModelResult = query.Select(i => new IdentityUserViewModel
            {
               Id = i.Id,
               Email = i.Email,
            }).ToList();


            return identityUserViewModelResult;
        }


    }
}
