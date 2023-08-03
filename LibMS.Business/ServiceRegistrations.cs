using System;
using LibMs.Data;
using LibMS.Business.Abstracts;
using LibMS.Business.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace LibMS.Business
{
	public static class ServiceRegistrations
	{
		public static void AddServiceRegistrations(this IServiceCollection serviceCollection)
		{
            serviceCollection.AddLibMSMappings();
            serviceCollection.AddScoped<IBookService, BookService>();
            serviceCollection.AddScoped<IAuthorService, AuthorService>();
            serviceCollection.AddScoped<IUserService, UserService>();
        }
    }
}

