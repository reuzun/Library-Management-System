using AutoMapper;
using LibMS.Persistance;
using LibMS.Persistance.EFConcreteRepositories;
using LibMS.Test.Persistance.MockEntity;
using Moq;
using Moq.EntityFrameworkCore;

namespace LibMS.Test.Persistance
{
    public class EFGenericRepositoryTests
    {
        private readonly Mock<LibMSContext> _myDbContextMock;
        private readonly IMapper _mapper;

        public EFGenericRepositoryTests()
        {
            _myDbContextMock = new Mock<LibMSContext>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<LibMS.Data.Mappings.AutoMapperConfiguration>()).CreateMapper();
        }

        [Fact]
        public async Task AsyncReadFirst_ShouldReturnFirstItem()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var entities = new List<Entity>() {
                new Entity() { Id = id, Name = "Entity 1" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 2" }
            };
            _myDbContextMock.Setup(x => x.Set<Entity>()).ReturnsDbSet(entities);

            var repository = new EFGenericRepository<Entity>(_myDbContextMock.Object, _mapper);

            // Act
            var result = await repository.AsyncReadFirst(e => e.Where(entity => entity.Id == id));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Entity 1", result.Name);
        }

        [Fact]
        public async Task AsyncCreate_ShouldReturnCreatedItem()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var entities = new List<Entity>() {
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 1" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 2" }
            };
            Entity entityToAdd = new Entity() { Id = id, Name = "Entity 3" };

            _myDbContextMock.Setup(x => x.Set<Entity>()).ReturnsDbSet(entities);

            var repository = new EFGenericRepository<Entity>(_myDbContextMock.Object, _mapper);

            // Act
            var result = await repository.AsyncCreate(entityToAdd);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Entity 3", result.Name);
        }

        [Fact]
        public async Task AsyncReadAll_NoQuery_ShouldReturnAllEntities()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var entities = new List<Entity>() {
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 1" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 2" }
            };

            _myDbContextMock.Setup(x => x.Set<Entity>()).ReturnsDbSet(entities);
            var repository = new EFGenericRepository<Entity>(_myDbContextMock.Object, _mapper);

            // Act
            var result = await repository.AsyncReadAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entities.Count, result.Count());
        }

        [Fact]
        public async Task AsyncReadAll_WithQuery_ShouldReturnFilteredEntities()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var entities = new List<Entity>() {
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 1" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 2" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 23" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 32" }
            };

            _myDbContextMock.Setup(x => x.Set<Entity>()).ReturnsDbSet(entities);
            var repository = new EFGenericRepository<Entity>(_myDbContextMock.Object, _mapper);

            // Act
            var result = await repository.AsyncReadAll(query => query.Where(entity => entity.Name.Contains('2')));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task AsyncRemove_ValidId()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var entities = new List<Entity>() {
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 1" },
                new Entity() { Id = id, Name = "Entity 2" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 3" },
            };

            _myDbContextMock.Setup(x => x.Set<Entity>()).ReturnsDbSet(entities);
            _myDbContextMock.Setup(x => x.Set<Entity>().FindAsync(It.IsAny<Guid>())).ReturnsAsync(entities[1]);

            var repository = new EFGenericRepository<Entity>(_myDbContextMock.Object, _mapper);

            // Act
            await repository.AsyncRemove(id);
            var result = await repository.AsyncReadAll();

            // Assert
            _myDbContextMock.Verify(e => e.Set<Entity>().Remove(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public async Task AsyncUpdate_ValidId_ShouldReturnUpdatedEntity()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var entities = new List<Entity>() {
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 1" },
                new Entity() { Id = id, Name = "Entity 2" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 3" },
            };

            _myDbContextMock.Setup(x => x.Set<Entity>()).ReturnsDbSet(entities);
            _myDbContextMock.Setup(x => x.Set<Entity>().FindAsync(It.IsAny<Guid>())).ReturnsAsync(entities[1]);

            var repository = new EFGenericRepository<Entity>(_myDbContextMock.Object, _mapper);

            // Act
            var result = await repository.AsyncUpdate(id, new Entity() { Name = "Entity 4" });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Entity 4", result.Name);
            Assert.NotEqual(result.Name, entities[1].Name);
        }

        [Fact]
        public async Task AsyncUpdate_NotValidId_ShouldReturnUpdatedEntity()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var entities = new List<Entity>() {
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 1" },
                new Entity() { Id = id, Name = "Entity 2" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 3" },
            };

            _myDbContextMock.Setup(x => x.Set<Entity>()).ReturnsDbSet(entities);
            _myDbContextMock.Setup(x => x.Set<Entity>().FindAsync(It.IsAny<Guid>())).ReturnsAsync((Entity)null);

            var repository = new EFGenericRepository<Entity>(_myDbContextMock.Object, _mapper);

            // Act
            var result = await repository.AsyncUpdate(id, new Entity() { Name = "Entity 4" });

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ReadAll_ShouldReturnAllEntities()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var entities = new List<Entity>() {
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 1" },
                new Entity() { Id = id, Name = "Entity 2" },
                new Entity() { Id = Guid.NewGuid(), Name = "Entity 3" },
            };

            _myDbContextMock.Setup(x => x.Set<Entity>()).ReturnsDbSet(entities);
            var repository = new EFGenericRepository<Entity>(_myDbContextMock.Object, _mapper);

            // Act
            var result = repository.ReadAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Count(), entities.Count);
        }
    }
}
