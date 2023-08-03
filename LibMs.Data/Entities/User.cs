using System;
namespace LibMs.Data.Entities
{
	public class User : IEntity
	{
		public Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public ICollection<Book> LoanedBooks { get; set; }
	}
}

