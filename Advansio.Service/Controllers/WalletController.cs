using Advansio.Core.Interfaces;
using Advansio.Core.Models;
using Advansio.Core.Utilities;
using Advansio.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Advansio.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class WalletController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletRepository _walletRepository;

        public WalletController(
            UserManager<ApplicationUser> userManager,
            IWalletRepository walletRepository
        )
        {
            _userManager = userManager;
            _walletRepository = walletRepository;
        }



        //GET : /api/Wallet/Balance
        [HttpGet]
        [Route("Balance")]
        public async Task<BasicResponse> GetBalance()  
        {
            var user = await GetUser();
            var wallet = await _walletRepository.Balance(user);
            if(wallet is null)
                return BasicResponse.FailureResponse("Wallet not created for this user");

            var data = new
            {
                user.Email,
                user.FirstName,
                user.LastName,
                wallet.WalletBalance
             
            };
            return BasicResponse.SuccessResponse("success", data);
        }

        //POST : /api/Wallet/FundTransfer
        [HttpPost("FundTransfer")]
        public async Task<BasicResponse> DebitWallet(TransferViewModel model)
        {
            var user = await GetUser();
            var totalAmount = model.Amount + model.Vat;
            return await _walletRepository.Debit(user, totalAmount);
        }

        async Task<ApplicationUser> GetUser()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }



    }
}
