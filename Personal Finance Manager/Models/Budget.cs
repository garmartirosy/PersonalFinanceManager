namespace Personal_Finance_Manager.Models
{
    public class Budget
    {
        public int Id { get; set; }

        public string OwnerUserId { get; set; } 

        public string Name { get; set; } 

        public decimal Balance { get; set; }

        public decimal TotalSpent { get; set; } = 0;

        public DateTime DateCreated { get; set; }

    }
}
