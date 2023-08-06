using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using LibMS.Persistance.EFConcreteRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibMS.Persistance
{
    public static class EFServiceRegistration
    {
        public static void AddEFRegistrations(this IServiceCollection services)
        {
            // Assuming all your entity types implement the IEntity interface
            IEnumerable<Type> entityTypes = typeof(IEntity).Assembly.GetTypes()
                .Where(t => typeof(IEntity).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (Type entityType in entityTypes)
            {
                Type repositoryType = typeof(EFGenericRepository<>).MakeGenericType(entityType);
                Type repositoryInterfaceType = typeof(IRepository<>).MakeGenericType(entityType);
                services.AddScoped(repositoryInterfaceType, repositoryType);
            }

            services.AddScoped<ITransactionManager, EFTransactionManager>();
        }
    }
}

