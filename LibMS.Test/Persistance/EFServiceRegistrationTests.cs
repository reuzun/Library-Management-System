using LibMS.Data.Entities;
using LibMS.Data.Mappings;
using LibMS.Data.Repositories;
using LibMS.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibMS.Test.Persistance
{
    public class EFServiceRegistrationTests
    {
        [Fact]
        public void AddEFRegistrations_ShouldRegisterRepositories()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<LibMSContext>(options => options.UseInMemoryDatabase(databaseName: "TestDb"));
            serviceCollection.AddAutoMapper(typeof(AutoMapperConfiguration));
            serviceCollection.AddEFRegistrations();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Act & Assert
            foreach (var entityType in typeof(IEntity).Assembly.GetTypes())
            {
                if (typeof(IEntity).IsAssignableFrom(entityType) && !entityType.IsInterface && !entityType.IsAbstract)
                {
                    var repositoryType = typeof(IRepository<>).MakeGenericType(entityType);
                    var registeredService = serviceProvider.GetService(repositoryType);
                    Assert.NotNull(registeredService);
                }
            }
        }
    }
}
