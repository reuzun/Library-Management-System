using System;
using LibMs.Data.Entities;

namespace LibMs.Data.Dtos
{
	public class BookDTO
	{
        public string? BookName { get; set; }
        public string? Description { get; set; }
        public ushort PageCount { get; set; }
        public DateTime PublishDate { get; set; }
        public byte LoanableCount { get; set; }
    }
}

