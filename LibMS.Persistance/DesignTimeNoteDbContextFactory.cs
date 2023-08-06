using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibMS.Persistance
{
    public class DesignTimeNoteDbContextFactory : IDesignTimeDbContextFactory<LibMSContext>
    {
        LibMSContext IDesignTimeDbContextFactory<LibMSContext>.CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<LibMSContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder
                .UseNpgsql("User Id=postgres; Password=123456; Host=localhost; Port=5432; DataBase=LibMS;");
            return new(dbContextOptionsBuilder.Options);
        }
    }
}

