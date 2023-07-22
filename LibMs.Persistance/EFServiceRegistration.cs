using LibMs.Data.Entities;
using LibMs.Data.Repositories;
using LibMs.Persistance.EFConcreteRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibMs.Persistance
{
    public static class EFServiceRegistration
    {
        public static void AddEFRegistrations(this IServiceCollection services)
        {
            services.AddDbContext<LibMSContext>((options) =>
            {
                options
                    .UseNpgsql("User Id=postgres; Password=123456; Host=localhost; Port=5432; DataBase=LibMS;");
            });

            // Assuming all your entity types implement the IEntity interface
            IEnumerable<Type> entityTypes = typeof(IEntity).Assembly.GetTypes()
                .Where(t => typeof(IEntity).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (Type entityType in entityTypes)
            {
                Type repositoryType = typeof(EFGenericRepository<>).MakeGenericType(entityType);
                Type repositoryInterfaceType = typeof(IRepository<>).MakeGenericType(entityType);
                services.AddScoped(repositoryInterfaceType, repositoryType);
            }
        }
    }
}

