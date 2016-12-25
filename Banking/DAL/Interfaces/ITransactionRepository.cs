using System.Collections.Generic;

namespace Banking.DAL.Interfaces
{
    public interface ITransactionRepository : IBaseReponsitory<Models.UserTransaction>
    {
        List<Models.UserTransaction> GetTransactionsByUser(int userId);
    }
}
