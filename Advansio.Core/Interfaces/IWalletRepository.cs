using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advansio.Core.Models;
using Advansio.Core.Utilities;
using Advansio.Core.ViewModels;

namespace Advansio.Core.Interfaces
{
    public interface IWalletRepository
    {

        Task<Wallet> CreateWallet(ApplicationUser user);

        Task<Wallet?> Balance(ApplicationUser user);

        Task<BasicResponse> Debit(ApplicationUser user, decimal totalAMount);

        Task<BasicResponse> Credit(ApplicationUser user, TransactionViewModel model);
    }
}
