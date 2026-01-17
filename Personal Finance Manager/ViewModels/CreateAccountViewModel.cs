using System.ComponentModel.DataAnnotations;

namespace Personal_Finance_Manager.ViewModels
{
    public class CreateAccountViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Account Type")]
        public int AccountTypeId { get; set; }

        public decimal Balance { get; set; }

    }
}
