using LibMS.Data.Entities;

namespace LibMS.Test.Persistance.MockEntity
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
