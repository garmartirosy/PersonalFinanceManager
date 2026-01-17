using Humanizer;
using System.Security.Principal;

namespace Personal_Finance_Manager.Models
{
    public class Account
    {
        public int Id {  get; set; }
        public string OwnerUserId { get; set; }
        public string Name { get; set; }

        public int AccountTypeId { get; set; }

        public string AccountType { get; set; }

        public decimal Balance { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsActive { get; set; }


    }
}
