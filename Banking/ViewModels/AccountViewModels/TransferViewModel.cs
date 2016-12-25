using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Banking.ViewModels.AccountViewModels
{
    public class TransferViewModel
    {
        public string FromAccountNumber { get; set; }

        public string CurrentBalance { get; set; }

        [Required]
        public string ToAccountNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
