using Microsoft.AspNetCore.Mvc;
using Personal_Finance_Manager.Data.Repositories;
using Personal_Finance_Manager.Models;
using Personal_Finance_Manager.Undo.Operations;
using Personal_Finance_Manager.Undo;
using Personal_Finance_Manager.ViewModels;
using Personal_Finance_Manager.Observers;
using Personal_Finance_Manager.Services;
using Personal_Finance_Manager.Services.Reports;
using System.Security.Claims;

namespace Personal_Finance_Manager.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly UndoService _undoService;
        private readonly TransactionEventBus _eventBus;
        private readonly MessageService _userMessages;
        private readonly ReportGenerator _reportGenerator;
        private string _userId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier); } }

        public TransactionController(
            ITransactionRepository transactionRepository,
            IAccountRepository accountRepository,
            UndoService undoService,
            TransactionEventBus eventBus,
            MessageService userMessages,
            ReportGenerator reportGenerator)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _undoService = undoService;
            _eventBus = eventBus;
            _userMessages = userMessages;
            _reportGenerator = reportGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int accountId = 0)
        {

            ViewData["HasUndo"] = _undoService.HasUndo(_userId);

            var msg = _userMessages.GetAndClear(_userId);
            if (msg != null)
                TempData["Message"] = msg;

            var transactions = await _transactionRepository.GetTransactionsByAccount(accountId, _userId);
            ViewData["currentAccountId"] = accountId;
            return View(transactions);
        }

        [HttpGet]
        public IActionResult CreateTransaction(int accountId)
        {
            var vm = new CreateTransactionViewModel();
            vm.AccountId = accountId;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(CreateTransactionViewModel transactionVM)
        {

            await _transactionRepository.CreateTransaction(transactionVM, _userId);

            await _eventBus.PublishAsync(new TransactionEvent(_userId, 0, transactionVM.AccountId, "Created"));

            return RedirectToAction(nameof(Index), new { accountId = transactionVM.AccountId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int transactionId, int? accountId)
        {
          
            var op = new DeleteTransactionOperation(_transactionRepository, _userId, transactionId);
            await _undoService.ExecuteAndRemember(_userId, op);

            await _eventBus.PublishAsync(new TransactionEvent(_userId, transactionId, accountId, "Deleted"));

            if (accountId.HasValue)
                return RedirectToAction(nameof(Index), new { accountId = accountId.Value });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UndoLast(int? accountId)
        {
            var undone = await _undoService.UndoLast(_userId);

            if (undone)
                await _eventBus.PublishAsync(new TransactionEvent(_userId, 0, accountId, "Undone"));

            if (accountId.HasValue)
                return RedirectToAction(nameof(Index), new { accountId = accountId.Value });

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Export(int accountId, string type = "csv")
        {
          
            var account = await _accountRepository.GetAccountById(accountId, _userId);
            if (account == null)
                return NotFound("Account not found.");

            var transactions = await _transactionRepository.GetTransactionsByAccount(accountId, _userId);
            var list = transactions?.ToList() ?? new List<Transaction>();

            ReportFile report = await _reportGenerator.GenerateReport(type, list, account.Name);

            return File(report.Bytes, report.ContentType, report.FileName);
        }
    }
}
