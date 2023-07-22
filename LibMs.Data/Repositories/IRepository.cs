using System;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;

namespace LibMs.Data.Repositories
{
	public interface IRepository<T> : IODataRepository<T> where T : IEntity, new()
    {
        public void AsyncCreate(T obj);
        public Task<T?> AsyncReadFirst(Func<IQueryable<T>, IQueryable<T>>? query = null);
        public Task<IEnumerable<T>> AsyncReadAll(Func<IQueryable<T>, IQueryable<T>>? query = null);
        public void AsyncRemove(Guid objId);
        public void AsyncUpdate(Guid objId, object obj);
    }
}

