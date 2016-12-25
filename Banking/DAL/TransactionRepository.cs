using System;
using System.Collections.Generic;
using System.Linq;
using Banking.DAL.Interfaces;
using Banking.Models;
using Microsoft.EntityFrameworkCore;

namespace Banking.DAL
{
    public class TransactionRepository : ITransactionRepository
    {
        private BankingDbContext context;

        public TransactionRepository(BankingDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<UserTransaction> GetAll()
        {
            return context.UserTransactions.ToList();
        }

        public UserTransaction GetById(int id)
        {
            return context.UserTransactions.Find(id);
        }

        public void Insert(UserTransaction entity)
        {
            context.UserTransactions.Add(entity);
        }

        public void Update(UserTransaction entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public List<UserTransaction> GetTransactionsByUser(int userId)
        {
            return context.UserTransactions.Where(_ => _.UserId == userId).OrderByDescending(_ => _.Id).ToList();
        }

        public void Delete(int id)
        {
            Models.UserTransaction student = context.UserTransactions.Find(id);
            context.UserTransactions.Remove(student);
        }

        public Models.UserTransaction FirstOrDefault(Func<UserTransaction, bool> func)
        {
            return context.UserTransactions.FirstOrDefault(func);
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