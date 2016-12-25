using System;
using System.Collections.Generic;
using System.Linq;
using Banking.DAL;
using Banking.DAL.Interfaces;
using Banking.Models;

namespace Banking.Bussiness
{
    public class AccountService
    {
        private static readonly object _lockAccount = new object();
        private readonly IAccountReponsitory _accountReponsitory;
        public AccountService(BankingDbContext context)
        {
            _accountReponsitory = new AccountRepository(context);
        }

       public Models.Account GetAccountInfo(string accountNumber)
        {
            var account = _accountReponsitory.FirstOrDefault(_ => _.AccountNumber == accountNumber);

            if (account == null)
            {
                throw new Exception("Account number does not exist!");
            }

            return account;
        }

        public void RegisterAccount(string accountNumber, string accountName, string password, decimal balance)
        {
            //protect concurrency duplicate account number
            lock (_lockAccount)
            {
                var account = _accountReponsitory.FirstOrDefault(_ => _.AccountNumber == accountNumber);
                if (account != null)
                {
                    throw new Exception("Account Number is already taken!");
                }

                account = new Models.Account()
                {
                    AccountName = accountName,
                    AccountNumber = accountNumber,
                    Password = password,
                    Balance = balance
                };

                _accountReponsitory.Insert(account);
                _accountReponsitory.Save();
            }
        }

        public Models.Account Login(string accountNumber, string password)
        {
            Models.Account account =
                _accountReponsitory.FirstOrDefault(_ => _.AccountNumber == accountNumber && _.Password == password);

            if (account == null)
            {
                throw new Exception("Account Number or password incorrect. Please try again");
            }

            return account;
        }
    }
}