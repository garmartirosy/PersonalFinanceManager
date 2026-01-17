namespace Personal_Finance_Manager.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        public string AccountName { get; set; }

        public string? Description { get; set; }

        public string DestinationName { get; set; } 

        public byte TransactionTypeId { get; set; }

        public DateTime DateCreated { get; set; }

        public decimal Amount { get; set; }

        public bool IsActive { get; set; } = true;

        public string? TransactionType { get; set; }
    }
}
