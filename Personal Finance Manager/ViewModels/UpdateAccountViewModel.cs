using System.ComponentModel.DataAnnotations;

namespace Personal_Finance_Manager.ViewModels
{
    public class UpdateAccountViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public int AccountTypeId { get; set; }

        public decimal Balance { get; set; }

    }
}
