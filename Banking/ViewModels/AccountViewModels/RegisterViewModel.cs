using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Banking.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string AccountNumber { get; set; }

        [Required]
        public string AccountName { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
