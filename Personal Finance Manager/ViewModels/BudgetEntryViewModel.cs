using Personal_Finance_Manager.Models;

namespace Personal_Finance_Manager.ViewModels
{
    public class BudgetEntryViewModel
    {
        public Budget Budget { get; set; }

        public List<BudgetEntry> Entries { get; set; } = new();
    }
}
