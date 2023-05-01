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
        List<IdentityUserViewModel> GetAllIdentityUsers();
        void SaveNewIdentityUser(IdentityUserViewModel identityUserViewModel);

    }
}
