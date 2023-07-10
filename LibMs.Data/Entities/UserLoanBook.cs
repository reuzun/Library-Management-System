using System;
namespace LibMs.Data.Entities
{
	public class UserLoanBook : IEntity
	{
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
	}
}

