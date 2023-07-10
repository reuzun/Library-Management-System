using LibMs.Data.Repositories;
using LibMs.Persistance.EFConcreteRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibMs.Persistance
{
    public static class EFServiceRegistration
    {
        public static void AddInfraServices(this IServiceCollection services)
        {
            services.AddDbContext<LibMSContext>((options) =>
            {
                options
                    .UseNpgsql("User Id=postgres; Password=123456; Host=localhost; Port=5432; DataBase=LibMS;");
            });

            services.AddScoped<IBookRepository, EFBookRepository>();
        }
    }
}

