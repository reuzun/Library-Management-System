using System;
using LibMS.Data.Entities;

namespace LibMS.Data.Dtos
{
	public class BookDTO
	{
        public string? BookName { get; set; }
        public string? Description { get; set; }
        public ushort PageCount { get; set; }
        public ushort PublishDate { get; set; }
        public byte LoanableCount { get; set; }
    }
}

