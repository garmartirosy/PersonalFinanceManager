using Microsoft.AspNetCore.Mvc;
using Personal_Finance_Manager.Models;
using Dapper;
using Personal_Finance_Manager.Data.Repositories;
using System.Security.Claims;
using Personal_Finance_Manager.ViewModels;
using Microsoft.Identity.Client;
using System.Security.Principal;

namespace Personal_Finance_Manager.Controllers
{
    public class AccountController : Controller
    {
        private IAccountRepository _accountRepository;
        private string _userId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier); } }

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var accounts = await _accountRepository.GetAccountsByUser(_userId);
            
            return View(accounts);
        }


        [HttpGet]
        public async Task<IActionResult> CreateAccount()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel accountVM)
        {


            await _accountRepository.CreateAccount(accountVM, _userId);

            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> UpdateAccount(int accountId)
        {
            var account = await _accountRepository.GetAccountById(accountId, _userId);

            var accountVM = new UpdateAccountViewModel()
            {
                Id = account.Id,
                Name = account.Name,
                AccountTypeId = account.AccountTypeId,
                Balance = account.Balance,
            };

            return View(accountVM);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateAccount(UpdateAccountViewModel accountVM)
        {
            
            var account = await _accountRepository.GetAccountById(accountVM.Id, _userId);

            account.Name = accountVM.Name;
            account.AccountTypeId = accountVM.AccountTypeId;
            account.Balance = accountVM.Balance;

            await _accountRepository.UpdateAccount(account);


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            bool succeeded = await _accountRepository.DeleteAccount(accountId, _userId);

            return RedirectToAction(nameof(Index));
        }


    }
}
