using System;
namespace LibMs.Data.Entities
{
	public class Author : IEntity
	{
		public Guid AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public DateTime BirthYear { get; set; }
        public IEnumerable<Book> WrittenBooks { get; set; }
	}
}

