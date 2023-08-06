using System;
namespace LibMS.Data.Entities
{
	public class BookAuthor
	{
        public Guid BookId { get; set; }
        public Book Book { get; set; }

        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
	}
}

