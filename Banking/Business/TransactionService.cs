using System;
using System.Collections.Generic;
using System.Linq;
using Banking.DAL;
using Banking.DAL.Interfaces;
using Banking.Models;

namespace Banking.Bussiness
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2
    }
    public class TransactionService
    {
        private static readonly object _lockTransaction = new object();
        private readonly IAccountReponsitory _accountReponsitory;
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(BankingDbContext context)
        {
            _accountReponsitory = new AccountRepository(context);
            _transactionRepository = new TransactionRepository(context);
        }

        public TransactionService(IAccountReponsitory accountReponsitory,
            ITransactionRepository transactionRepository)
        {
            _accountReponsitory = accountReponsitory;
            _transactionRepository = transactionRepository;
        }

        public List<UserTransaction> GetUserTransactions(int userId)
        {
            var account = _accountReponsitory.FirstOrDefault(_ => _.Id == userId);

            if (account == null)
            {
                throw new Exception("Account does not exist!");
            }

            return _transactionRepository.GetTransactionsByUser(userId);
        }
        public void Withdraw(string accountNumber, decimal amount)
        {
            lock (_lockTransaction)
            {
                var account = _accountReponsitory.FirstOrDefault(_ => _.AccountNumber == accountNumber);

                if (account == null)
                {
                    throw new Exception("Account number does not exist!");
                }

                if (account.Balance < amount)
                {
                    throw new Exception("Balance too low.");
                }
                var startBalance = account.Balance;

                account.Balance -= amount;

                var userTrans = new UserTransaction()
                {
                    UserId = account.Id,
                    StartBalance = startBalance,
                    EndBalance = account.Balance,
                    Amount = amount,
                    TransactionNo = Guid.NewGuid().ToString(),
                    TransactionType = (int)TransactionType.Withdraw
                };

                _accountReponsitory.Update(account);
                _transactionRepository.Insert(userTrans);
                _accountReponsitory.Save();
                _transactionRepository.Save();
            }
        }

        public void Deposit(string accountNumber, decimal amount)
        {
            lock (_lockTransaction)
            {
                var account = _accountReponsitory.FirstOrDefault(_ => _.AccountNumber == accountNumber);
                if (account == null)
                {
                    throw new Exception("Account number does not exist!");
                }
                var startBalance = account.Balance;
                account.Balance += amount;

                var userTrans = new UserTransaction()
                {
                    UserId = account.Id,
                    StartBalance = startBalance,
                    EndBalance = account.Balance,
                    Amount = amount,
                    TransactionNo = Guid.NewGuid().ToString(),
                    TransactionType = (int)TransactionType.Deposit
                };

                _accountReponsitory.Update(account);
                _accountReponsitory.Save();

                _transactionRepository.Insert(userTrans);
                _transactionRepository.Save();
            }
        }

        public void Transfer(string fromAccountNumber, string toAccountNumber, decimal amount)
        {
            lock (_lockTransaction)
            {
                var accountSource = _accountReponsitory.FirstOrDefault(_ => _.AccountNumber == fromAccountNumber);
                if (accountSource == null)
                {
                    throw new Exception("Source account number does not exist!");
                }

                var accountDestin = _accountReponsitory.FirstOrDefault(_ => _.AccountNumber == toAccountNumber);
                if (accountDestin == null)
                {
                    throw new Exception("Destination Account number does not exist!");
                }

                if (accountSource.Balance < amount)
                {
                    throw new Exception("Balance too low.");
                }

                var startBalanceSource = accountSource.Balance;
                var startBalanceDestin = accountDestin.Balance;
                accountSource.Balance -= amount;
                accountDestin.Balance += amount;

                var tranAccountSource = new UserTransaction()
                {
                    UserId = accountSource.Id,
                    StartBalance = startBalanceSource,
                    EndBalance = accountSource.Balance,
                    TransactionNo = Guid.NewGuid().ToString(),
                    Amount = amount,
                    TransactionType = (int)TransactionType.Withdraw
                };

                var tranAccountDestin = new UserTransaction()
                {
                    UserId = accountDestin.Id,
                    StartBalance = startBalanceDestin,
                    EndBalance = accountDestin.Balance,
                    TransactionNo = Guid.NewGuid().ToString(),
                    Amount = amount,
                    TransactionType = (int)TransactionType.Deposit
                };

                _accountReponsitory.Update(accountSource);
                _accountReponsitory.Update(accountDestin);
                _accountReponsitory.Save();

                _transactionRepository.Insert(tranAccountSource);
                _transactionRepository.Insert(tranAccountDestin);
                _transactionRepository.Save();
            }
        }
    }
}