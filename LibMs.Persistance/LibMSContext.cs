using System;
using LibMs.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibMs.Persistance
{
	public class LibMSContext : DbContext
	{
        DbSet<Book> Books { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Author> Authors { get; set; }
        DbSet<BookAuthor> BookAuthors { get; set; }
        DbSet<UserLoanBook> UserLoanBooks { get; set; }

        public LibMSContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(user => user.UserId);


            modelBuilder.Entity<Author>()
                .HasKey(author => author.AuthorId);


            modelBuilder.Entity<Book>()
                .HasKey(book => book.BookId);

            modelBuilder.Entity<Book>()
                .Property(book => book.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<BookAuthor>()
                .HasKey(p => new { p.AuthorId, p.BookId });

            modelBuilder.Entity<UserLoanBook>()
                .HasKey(p => new { p.UserId, p.BookId });
        }
    }
}

