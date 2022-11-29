using Advansio.Core.Interfaces;
using Advansio.Core.Models;
using Advansio.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Advansio.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWalletRepository _walletRepository;

        public DashboardController(UserManager<ApplicationUser> userManager,
            IWalletRepository walletRepository
        )
        {
            _userManager      = userManager;
            _walletRepository = walletRepository;
        }

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user =  await GetUser();
       
            var wallet = await _walletRepository.Balance(user);
            return View(wallet);
        }


        [HttpGet]
        public async Task<IActionResult> Transfer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            if (ModelState.IsValid)
            {
                var totalAmount = model.Amount + model.Vat;

                var user = await GetUser();
                
                var response = await _walletRepository.Debit(user, totalAmount);

                if (response.Success)
                {
                    return RedirectToAction("Success");
                }
                
                ViewBag.Error = response.Message;
                return View("Transfer");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

        async Task<ApplicationUser> GetUser()
        {
            string userId = User.Identity.Name;
            var user = await _userManager.FindByEmailAsync(userId);
            return user;
        }


    }
}
