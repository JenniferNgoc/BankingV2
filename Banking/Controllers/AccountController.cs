using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Banking.Bussiness;
using Banking.Helper;
using Banking.Models;
using Banking.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Banking.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankingDbContext _context;
    
        public AccountController(BankingDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    new AccountService(_context).RegisterAccount(model.AccountNumber, model.AccountName, model.Password, model.Balance);
                    ModelState.AddModelError(string.Empty, "Your account has been created successfully and is ready to use");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            HttpContext.Authentication.SignOutAsync("Cookies");
            return View();
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
              var acc=  new AccountService(_context).Login(model.AccountNumber, model.Password);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, acc.AccountNumber),
                    new Claim(ClaimTypes.NameIdentifier, acc.Id.ToString())
                };
                
                var id = new ClaimsIdentity(claims, "USER_INFO");
                var p = new ClaimsPrincipal(id);

                HttpContext.Authentication.SignInAsync("Cookies", p);
                return RedirectToAction("Manage", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }

        [Authorize]
        public IActionResult Manage()
        {
            var listTran = new TransactionService(_context).GetUserTransactions(User.GetUserId());
            ManageViewModel manageViewModel = new ManageViewModel()
            {
                UserTransactions = listTran
            };
            return View(manageViewModel);
        }

        [Authorize]
        public IActionResult Deposit()
        {
            var accInfo = new AccountService(_context).GetAccountInfo(User.GetAccNo());
            DepositViewModel depositViewModel = new DepositViewModel()
            {
                AccountNumber = accInfo.AccountNumber,
                CurrentBalance = accInfo.Balance.Value.ToString("C"),
                Amount = 0
            };
            return View(depositViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Deposit(DepositViewModel model)
        {
            try
            {
                new TransactionService(_context).Deposit(User.GetAccNo(), model.Amount);
                var accInfo = new AccountService(_context).GetAccountInfo(User.GetAccNo());
                model = new DepositViewModel()
                {
                    AccountNumber = accInfo.AccountNumber,
                    CurrentBalance = accInfo.Balance.Value.ToString("C"),
                    Amount = 0
                };
                ModelState.AddModelError(string.Empty, "Deposit request successfully submitted");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            
            return View(model);
        }

        [Authorize]
        public IActionResult Transfer()
        {
            var accInfo = new AccountService(_context).GetAccountInfo(User.GetAccNo());
            TransferViewModel withdrawViewModel = new TransferViewModel()
            {
                FromAccountNumber = accInfo.AccountNumber,
                CurrentBalance = accInfo.Balance.Value.ToString("C"),
                Amount = 0
            };
            return View(withdrawViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Transfer(TransferViewModel model)
        {
            try
            {
                new TransactionService(_context).Transfer(User.GetAccNo(), model.ToAccountNumber, model.Amount);
                var accInfo = new AccountService(_context).GetAccountInfo(User.GetAccNo());
                model = new TransferViewModel()
                {
                    FromAccountNumber = accInfo.AccountNumber,
                    CurrentBalance = accInfo.Balance.Value.ToString("C"),
                    Amount = 0
                };
                ModelState.AddModelError(string.Empty, "Your request is successfully submitted");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }

        [Authorize]
        public IActionResult Withdraw()
        {
            var accInfo = new AccountService(_context).GetAccountInfo(User.GetAccNo());
            WithdrawViewModel withdrawViewModel = new WithdrawViewModel()
            {
                AccountNumber = accInfo.AccountNumber,
                CurrentBalance = accInfo.Balance.Value.ToString("C"),
                Amount = 0
            };
            return View(withdrawViewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Withdraw(WithdrawViewModel model)
        {
            try
            {
                new TransactionService(_context).Withdraw(User.GetAccNo(), model.Amount);
                var accInfo = new AccountService(_context).GetAccountInfo(User.GetAccNo());
                model = new WithdrawViewModel()
                {
                    AccountNumber = accInfo.AccountNumber,
                    CurrentBalance = accInfo.Balance.Value.ToString("C"),
                    Amount = 0
                };
                ModelState.AddModelError(string.Empty, "Your withdrawal request is successfully submitted");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }
    }
}
