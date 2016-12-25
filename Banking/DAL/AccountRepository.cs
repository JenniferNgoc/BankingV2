using System;
using System.Collections.Generic;
using System.Linq;
using Banking.DAL.Interfaces;
using Banking.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.DAL
{
    public class AccountRepository : IAccountReponsitory
    {
        private BankingDbContext context;

        public AccountRepository(BankingDbContext context)
        {
            this.context = context;
        }
       
        public IEnumerable<Account> GetAll()
        {
            return context.Accounts.ToList();
        }

        public Account GetById(int id)
        {
            return context.Accounts.Find(id);
        }

        public void Insert(Models.Account entity)
        {
            context.Accounts.Add(entity);
        }

        public void Update(Account entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Account student = context.Accounts.Find(id);
            context.Accounts.Remove(student);
        }

        public Account FirstOrDefault(Func<Account, bool> func)
        {
            return context.Accounts.FirstOrDefault(func);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}