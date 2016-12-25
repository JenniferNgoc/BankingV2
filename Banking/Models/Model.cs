using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options)
            : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserTransaction> UserTransactions { get; set; }
    }

    [Table("Account")]
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public decimal? Balance { get; set; }
    }

    [Table("UserTransaction")]
    public class UserTransaction
    {
        public int Id { get; set; }
        public string TransactionNo { get; set; }
        public int? UserId { get; set; }
        public int? TransactionType { get; set; }
        public decimal? StartBalance { get; set; }
        public decimal? EndBalance { get; set; }
        public decimal? Amount { get; set; }
    }
}
