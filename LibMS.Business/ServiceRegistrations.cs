using System;
using LibMS.Business.Abstracts;
using LibMS.Business.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace LibMS.Business
{
	public static class ServiceRegistrations
	{
		public static void AddServiceRegistrations(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IBookService, BookService>();
		}
	}
}

