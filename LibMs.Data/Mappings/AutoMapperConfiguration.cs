using System;
using AutoMapper;
using LibMs.Data.Dtos;
using LibMs.Data.Entities;

namespace LibMs.Data.Mappings
{
	public class AutoMapperConfiguration : Profile
	{
		public AutoMapperConfiguration()
		{
			CreateMap<Book, BookDTO>();
			CreateMap<BookDTO, Book>()
				.ForMember(
					book => book.BookId,
					opt => opt.MapFrom((src) => new Guid())
				);
			CreateMap<Book, Book>()
				.ForMember(
					book => book.BookId,
					opt => opt.Ignore()
				)
                .ForMember(dest => dest.BookName, opt => opt.Condition(src => src.BookName != null))
                .ForMember(dest => dest.PublishDate, opt => opt.Condition(src => src.PublishDate != 0))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
                .ForMember(dest => dest.PageCount, opt => opt.Condition(src => src.PageCount != 0))
                .ForMember(dest => dest.TotalCount, opt => opt.Condition(src => src.TotalCount != 0))
                .ForMember(dest => dest.LoanableCount, opt => opt.Condition(src => src.LoanableCount != 0))
                .ForMember(dest => dest.Authors, opt => opt.Condition(src => src.Authors != null))
                .ForMember(dest => dest.Users, opt => opt.Condition(src => src.Users != null));
			CreateMap<Author, Author>()
				.ForMember(dest => dest.AuthorName, opt => opt.Condition(src => src.AuthorName != null))
				.ForMember(dest => dest.BirthYear, opt => opt.Condition(src => src.BirthYear != null))
				.ForMember(dest => dest.WrittenBooks, opt => opt.Condition(src => src.WrittenBooks != null));
            CreateMap<AuthorDTO, Author>()
                .ForMember(
                    author => author.AuthorId,
                    opt => opt.MapFrom((src) => new Guid())
                );
			CreateMap<UserDTO, User>()
                .ForMember(
                    user => user.UserId,
                    opt => opt.MapFrom((src) => new Guid())
                );

        }
    }
}

