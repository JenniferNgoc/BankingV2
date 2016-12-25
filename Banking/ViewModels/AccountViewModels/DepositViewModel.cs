using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Banking.ViewModels.AccountViewModels
{
    public class DepositViewModel
    {
        public string AccountNumber { get; set; }

        public string CurrentBalance { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
