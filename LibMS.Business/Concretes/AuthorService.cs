using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LibMS.Data.Dtos;
using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using LibMS.Business.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace LibMS.Business.Concretes
{
    public class AuthorService : IAuthorService
    {
        IRepository<Author> _authorRepository;
        IMapper _mapper;

        public AuthorService(IRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<Author> AddAuthorAsync(AuthorDTO authorDto)
        {
            if(authorDto.AuthorName == null || authorDto.AuthorName.Length == 0)
            {
                throw new ValidationException("AuthorName Shouldnt be null or Length shouldnt be equal 0.");
            }

            var author = _mapper.Map<Author>(authorDto);
            return await _authorRepository.AsyncCreate(author);
        }

        public async Task<Author> GetAuthorById(Guid authorId)
        {
            var author = await _authorRepository.AsyncReadFirst(a => a.Where(author => author.AuthorId == authorId));
            if(author != null)
            {
                return author;
            }
            else
            {
                throw new Exception("No such author");
            }
        }

        public IQueryable<Author> GetAuthors(Func<IQueryable<Author>, IQueryable<Author>>? query = null)
        {
            if (query != null)
            {
                return query(_authorRepository.ReadAll().Include(author => author.WrittenBooks));
            }
            return _authorRepository.ReadAll().Include(author => author.WrittenBooks);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync(Func<IQueryable<Author>, IQueryable<Author>>? query = null)
        {
            if (query != null)
            {
                return (await _authorRepository.AsyncReadAll(b => query(b.Include(author => author.WrittenBooks))));
            }
            return (await _authorRepository.AsyncReadAll(b => b.Include(author => author.WrittenBooks)));
        }

        public async Task<Author> UpdateAuthorAsync(Guid id, AuthorDTO authorDto)
        {
            return await _authorRepository.AsyncUpdate(id, _mapper.Map(authorDto, new Author()));
        }
    }
}

