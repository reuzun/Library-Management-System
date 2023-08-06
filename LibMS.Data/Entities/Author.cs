using System;
using System.ComponentModel.DataAnnotations;

namespace LibMS.Data.Entities
{
	public class Author : IEntity
	{
		public Guid AuthorId { get; set; }
		[Required]
		[MinLength(1)]
        public string? AuthorName { get; set; }
        public DateTime BirthYear { get; set; }
        public IEnumerable<Book> WrittenBooks { get; set; }
	}
}

