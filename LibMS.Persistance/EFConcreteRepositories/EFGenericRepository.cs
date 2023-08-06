using System;
using LibMS.Data;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace LibMS.Persistance.EFConcreteRepositories
{
    public class EFGenericRepository<T> : IODataRepository<T>, IRepository<T> where T : class, IEntity, new()
    {
        LibMSContext _context;
        IMapper _mapper;

        public EFGenericRepository(LibMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<T> AsyncCreate(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
            await _context.SaveChangesAsync();
            return obj;
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

        public async Task<T> AsyncUpdate(Guid objId, T obj)
        {
            var entity = await _context.Set<T>().FindAsync(objId);
            entity = _mapper.Map(obj, entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<T> ReadAll()
        {
            return _context.Set<T>();
        }
    }
}

