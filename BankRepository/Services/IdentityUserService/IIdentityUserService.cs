using BankRepository.ViewModels.IdentityUserView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankRepository.Services.IdentityUserService
{
    public interface IIdentityUserService
    {
        void SaveNewIdentityUser(IdentityUserViewModel identityUserViewModel);

        List<IdentityUserViewModel> GetAllIdentityUsers();
    }
}
