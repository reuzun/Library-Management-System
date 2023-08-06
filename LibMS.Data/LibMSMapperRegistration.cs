using System;
using System.Reflection;
using LibMS.Data.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace LibMS.Data
{
	public static class LibMSMapperRegistration
	{
		public static void AddLibMSMappings(this IServiceCollection collection)
		{
			collection.AddAutoMapper(typeof(AutoMapperConfiguration));
		}
	}
}

