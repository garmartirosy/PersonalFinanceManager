using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Personal_Finance_Manager.ViewModels
{
    public class CreateTransactionViewModel
    {
        public int AccountId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [DisplayName("Transaction Type")]
        public int TransactionTypeId { get; set; }

        [Required]
        [DisplayName("Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [DisplayName("Destination Name")]
        public string? DestinationName { get; set; }

        public string? Description { get; set; }
    }
}
