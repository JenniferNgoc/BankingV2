using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Banking.Models;

namespace Banking.ViewModels.AccountViewModels
{
    public class ManageViewModel
    {
        public List<UserTransaction> UserTransactions { get; set; }
        
    }
}
