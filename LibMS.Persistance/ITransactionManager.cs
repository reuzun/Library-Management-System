using System;
using System.Data;

namespace LibMs.Persistance
{
	public interface ITransactionManager
	{
		void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
		void CommitTransaction();
		void RollbackTransaction();

        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}

