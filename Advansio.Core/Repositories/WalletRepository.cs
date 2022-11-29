using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advansio.Core.Data;
using Advansio.Core.Interfaces;
using Advansio.Core.Models;
using Advansio.Core.Utilities;
using Advansio.Core.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Advansio.Core.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly Context _context;

        public WalletRepository(Context context
        )
        {
            _context = context;
        }


        public async Task<Wallet> CreateWallet(ApplicationUser user)
        {
            var newWallet = new Wallet()
            {
                UserId = user.Id,
                WalletBalance = 5000,
                DateCreated = DateTime.Now
            };
            await _context.Wallets.AddAsync(newWallet);
            await _context.SaveChangesAsync();

            return newWallet;

        }


        public async Task<Wallet?> Balance(ApplicationUser user)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.UserId == user.Id);
            return wallet;

        }

        public async Task<BasicResponse> Debit(ApplicationUser user, decimal totalAmount)
        {
            var wallet = _context.Wallets.FirstOrDefault(x => x.UserId == user.Id);

            if (wallet == null)
                return BasicResponse.FailureResponse("Wallet not created for this user");

            if (totalAmount > wallet.WalletBalance)
                return BasicResponse.FailureResponse("Insufficient balance");

            var balance = wallet.WalletBalance - totalAmount;
            // Send amount to  account here

            wallet.WalletBalance = balance;
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();

            var data = new
            {
                user.Email,
                user.FirstName,
                user.LastName,
                NewBalannce = wallet.WalletBalance
            };
            return BasicResponse.SuccessResponse("Successful", data);

        }


        public async Task<BasicResponse> Credit(ApplicationUser user, TransactionViewModel model)
        {
            var wallet = _context.Wallets.FirstOrDefault(x => x.UserId == user.Id);

            if (wallet == null)
                return BasicResponse.FailureResponse("Wallet not created for this user");

            var newBalance = wallet.WalletBalance + model.Amount;

            wallet.WalletBalance = newBalance;
            await _context.Wallets.AddAsync(wallet);
            await _context.SaveChangesAsync();

            return BasicResponse.SuccessResponse("Successful", wallet);
        }


    }
}
