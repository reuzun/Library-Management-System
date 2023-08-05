using System;
using System.Reflection;
using LibMs.Data.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace LibMs.Data
{
	public static class LibMSMapperRegistration
	{
		public static void AddLibMSMappings(this IServiceCollection collection)
		{
			collection.AddAutoMapper(typeof(AutoMapperConfiguration));
		}
	}
}

