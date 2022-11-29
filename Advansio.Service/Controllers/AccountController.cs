using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Advansio.Core.Data;
using Advansio.Core.Interfaces;
using Advansio.Core.Models;
using Advansio.Core.Utilities;
using Advansio.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Advansio.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWalletRepository _walletRepository;

        public AccountController(
            IConfiguration config, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWalletRepository walletRepository
           )
        {
            _config           = config;
            _userManager      = userManager;
            _signInManager    = signInManager;
            _walletRepository = walletRepository;
        }


        //POST : /api/Account/Register
        [HttpPost("Register")]
        public async Task<BasicResponse> Register(RegisterViewModel model)
        { 
            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var wallet = await _walletRepository.CreateWallet(user);

                if (wallet is not null)
                {
                    var data = new
                     {
                         wallet.WalletBalance,
                         model.FirstName,
                         model.LastName,
                         model.Email
                     };

                    return BasicResponse.SuccessResponse("success", data);
                }
                return BasicResponse.FailureResponse("failed to create wallet");
               
            }
            return BasicResponse.FailureResponse(result.ToString());

        }

        //POST : api/Account/Login
        [HttpPost("Login")]
        public async Task<BasicResponse> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);   //    FindByNameAsync(model.Email); 
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await GenerateJwtToken(model);
                var data = new
                {
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    Token = token
                };
                return BasicResponse.SuccessResponse("success", data);
            }
            else return BasicResponse.FailureResponse("Username or password is incorrect");

        }


        async Task<string> GenerateJwtToken(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var securityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecurityKey"]));


            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Name", user.Email),
                    new Claim("UserID", user.Id),
                   // new Claim("FirstName", user?.FirstName),
                   // new Claim("LastName", user?.LastName),
                    new Claim("Email", user.Email),
                }),
                Expires =  DateTime.UtcNow.AddMinutes(10),  // DateTime.UtcNow.AddDays(1)
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return  token;
        }




    }
}
