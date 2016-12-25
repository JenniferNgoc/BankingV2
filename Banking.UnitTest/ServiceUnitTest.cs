using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Banking.Bussiness;
using Banking.DAL.Interfaces;
using Banking.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Banking.UnitTest
{
    public class ServiceUnitTest
    {
        [Fact]
        public void TransferConcurencyUnitTest1()
        {
            var _accountDataTest = new List<Models.Account>()
            {
                new Models.Account {AccountNumber = "123", Balance = 100000},
                new Models.Account {AccountNumber = "1234", Balance = 200000},
                new Models.Account {AccountNumber = "12345", Balance = 500000},
                new Models.Account {AccountNumber = "123456", Balance = 0}
            };

            var _accountReponsitory = new Mock<IAccountReponsitory>();
            var _transactionReponsitory = new Mock<ITransactionRepository>();
          
            _accountReponsitory.Setup(r => r.FirstOrDefault(It.IsAny<Func<Models.Account, bool>>()))
                .Returns((Func<Models.Account, bool> expr) => _accountDataTest.FirstOrDefault(expr));

            _accountReponsitory.Setup(mr => mr.Update(It.IsAny<Models.Account>())).Verifiable();

            TransactionService transactionService = new TransactionService(_accountReponsitory.Object, _transactionReponsitory.Object);
            
            Task task1 = Task.Run(() =>
            {
                Parallel.For(0, 25,
                                index =>
                                {
                                    transactionService.Transfer("123", "123456", index);
                                });
            });

            Task task2 = Task.Run(() =>
            {
                Parallel.For(0, 40,
                                index =>
                                {
                                    transactionService.Transfer("1234", "123456", index);
                                });
            });

            Task task3 = Task.Run(() =>
            {
                Parallel.For(0, 40,
                                index =>
                                {
                                    transactionService.Transfer("12345", "123456", index);
                                });
            });
            transactionService.Transfer("12345", "123456", 10000);

            Task.WaitAll(task1, task2, task3);

            var sumTotal = _accountDataTest.Sum(x => x.Balance);
            var sumSourceBalaceAcc = _accountDataTest.Where(a => a.AccountNumber != "123456").Sum(x => x.Balance);
            Assert.Equal(sumTotal, 800000);
            Assert.Equal(800000 - sumSourceBalaceAcc, _accountDataTest.Find(_ => _.AccountNumber == "123456").Balance);
        }
    }
}
