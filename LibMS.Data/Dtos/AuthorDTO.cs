using System;
using System.ComponentModel.DataAnnotations;

namespace LibMS.Data.Dtos
{
	public class AuthorDTO
	{
        public string? AuthorName { get; set; }
        public DateTime BirthYear { get; set; }
	}
}

