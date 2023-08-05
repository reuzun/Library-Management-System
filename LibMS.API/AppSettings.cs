using System;
using LibMs.API.Settings;

namespace LibMs.API
{
	public class AppSettings
	{
		public DatabaseSettings Database { get; set; }
		public CacheSettings CacheSettings { get; set; }
		public ushort AllowedBookLoanCount { get; set; }
    }
}

