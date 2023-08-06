using System;
using System.Data;
using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibMS.Persistance
{
    public class EFTransactionManager : ITransactionManager
    {
        LibMSContext _context;

        public EFTransactionManager(LibMSContext context)
        {
            _context = context;
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _context.Database.BeginTransaction(isolationLevel);
        }

        public Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            return _context.Database.BeginTransactionAsync(isolationLevel);
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public Task CommitTransactionAsync()
        {
            return _context.Database.CommitTransactionAsync();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }

        public Task RollbackTransactionAsync()
        {
            return _context.Database.RollbackTransactionAsync();
        }
    }
}

