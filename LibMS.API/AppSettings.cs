using System;
using LibMS.API.Settings;

namespace LibMS.API
{
	public class AppSettings
	{
		public DatabaseSettings Database { get; set; }
		public CacheSettings CacheSettings { get; set; }
		public ushort AllowedBookLoanCount { get; set; }
    }
}

