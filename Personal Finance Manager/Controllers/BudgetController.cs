using Microsoft.AspNetCore.Mvc;
using Personal_Finance_Manager.Data.Repositories;
using Personal_Finance_Manager.Models;
using System.Security.Claims;
using Personal_Finance_Manager.ViewModels;

namespace Personal_Finance_Manager.Controllers
{
    public class BudgetController : Controller
    {
        private IBudgetRepository _budgetRepository;
        private string _userId { get { return User.FindFirstValue(ClaimTypes.NameIdentifier); } }

        public BudgetController(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var budgets = await _budgetRepository.GetBudgetsByUser(_userId);

            foreach (var budget in budgets)
            {
                budget.TotalSpent = await _budgetRepository.GetTotalSpent(budget.Id);
            }

            return View(budgets);
        }


        [HttpGet]
        public async Task<IActionResult> CreateBudget()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget(CreateBudgetViewModel budgetVM)
        {
            var budget = new Budget()
            {
                OwnerUserId = _userId,
                Name = budgetVM.Name,
                Balance = budgetVM.Balance
            };

            await _budgetRepository.CreateBudget(budget);

            return View();
        }

 
        [HttpPost]
        public async Task<IActionResult> DeleteBudget(int budgetId)
        {
            await _budgetRepository.DeleteBudget(budgetId, _userId);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> GetBudgetEntries(int budgetId)
        {
            var budget = await _budgetRepository.GetBudgetById(budgetId, _userId);

            budget.TotalSpent = await _budgetRepository.GetTotalSpent(budgetId);

            var entries = await _budgetRepository.GetBudgetEntriesByBudget(budgetId, _userId);

            var vm = new BudgetEntryViewModel
            {
                Budget = budget,
                Entries = entries.ToList()
            };

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> CreateBudgetEntry(int budgetId)
        {
            var budget = await _budgetRepository.GetBudgetById(budgetId, _userId);

            var entryVM = new CreateBudgetEntryViewModel()
            {
                BudgetId = budgetId,
                BudgetName = budget.Name
            };

            return View(entryVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudgetEntry(CreateBudgetEntryViewModel entryVM)
        {
            var entry = new BudgetEntry()
            {
                BudgetId = entryVM.BudgetId,
                OwnerUserId = _userId,
                Name = entryVM.Name,
                Amount = entryVM.Amount
            };

            await _budgetRepository.CreateBudgetEntry(entry);

            return RedirectToAction(nameof(GetBudgetEntries), new { budgetId = entryVM.BudgetId });
        }

      
        [HttpPost]
        public async Task<IActionResult> DeleteBudgetEntry(int budgetEntryId, int budgetId)
        {
            await _budgetRepository.DeleteBudgetEntry(budgetEntryId, _userId);

            return RedirectToAction(nameof(GetBudgetEntries), new { budgetId = budgetId });
        }
    }
}
