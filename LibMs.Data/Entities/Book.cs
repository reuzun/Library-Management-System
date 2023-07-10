﻿namespace LibMs.Data.Entities
{
	public class Book : IEntity
	{
        public Guid BookId { get; set; }
        public string? BookName { get; set; }
        public string? Description { get; set; }
        public ushort PageCount { get; set; }
        public DateTime PublishDate { get; set; }
        public byte LoanableCount { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<User> Users { get; set; }
	}
}

