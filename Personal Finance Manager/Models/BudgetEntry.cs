namespace Personal_Finance_Manager.Models
{
    public class BudgetEntry
    {
        public int Id { get; set; }

        public int BudgetId { get; set; }

        public string OwnerUserId { get; set; }

        public string Name { get; set; } 

        public decimal Amount { get; set; }
    }
}
