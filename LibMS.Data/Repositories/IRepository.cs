using System;
using LibMS.Data;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;

namespace LibMS.Data.Repositories
{
	public interface IRepository<T> : IODataRepository<T> where T : IEntity, new()
    {
        public Task<T> AsyncCreate(T obj);
        public Task<T?> AsyncReadFirst(Func<IQueryable<T>, IQueryable<T>>? query = null);
        public Task<IEnumerable<T>> AsyncReadAll(Func<IQueryable<T>, IQueryable<T>>? query = null);
        public Task AsyncRemove(Guid objId);
        public Task<T> AsyncUpdate(Guid objId, T obj);
    }
}

