using System;
namespace LibMS.Data.Entities
{
	public class UserLoanBook
	{
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
	}
}

