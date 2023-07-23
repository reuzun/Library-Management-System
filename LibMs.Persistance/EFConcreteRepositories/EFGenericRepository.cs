using System;
using LibMs.Data;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;
using LibMs.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibMs.Persistance.EFConcreteRepositories
{
    public class EFGenericRepository<T> : IODataRepository<T>, IRepository<T> where T : class, IEntity, new()
    {
        LibMSContext _context;

        public EFGenericRepository(LibMSContext context)
        {
            _context = context;
        }

        public void AsyncCreate(T obj)
        {
            _context.Set<T>().AddAsync(obj);
            _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> AsyncReadAll(Func<IQueryable<T>, IQueryable<T>>? query = null)
        {
            IQueryable<T> rows = _context.Set<T>();

            if (query != null)
            {
                rows = query(rows);
            }

            return await rows.ToListAsync();
        }

        public async Task<T?> AsyncReadFirst(Func<IQueryable<T>, IQueryable<T>>? query = null)
        {
            IQueryable<T> rows = _context.Set<T>();

            if (query != null)
            {
                rows = query(rows);
            }

            return await rows.FirstOrDefaultAsync();
        }

        public async Task AsyncRemove(Guid objId)
        {
            var book = await _context.Set<T>().FindAsync(objId);
            if (book != null)
            {
                _context.Set<T>().Remove(book);
                try
                {
                    await _context.SaveChangesAsync();
                }catch(Exception e)
                {
                    Console.WriteLine("------------------");
                    Console.WriteLine(e);
                    Console.WriteLine("------------------");
                }
            }
        }

        public void AsyncUpdate(Guid objId, object obj)
        {
            throw new NotImplementedException(); ;
        }

        public IQueryable<T> ReadAll()
        {
            return _context.Set<T>();
        }
    }
}

