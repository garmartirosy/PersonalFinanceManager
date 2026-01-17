namespace Personal_Finance_Manager.ViewModels
{
    public class CreateBudgetEntryViewModel
    {
        public int BudgetId { get; set; }

        public string BudgetName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }
    }
}
