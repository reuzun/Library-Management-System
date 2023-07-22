
using LibMs.Persistance;
using LibMS.Business;
using Microsoft.AspNetCore.OData;
using Serilog;

namespace LibMs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((hostContext, services, configuration) =>
            {
                configuration.WriteTo.Console();
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddEFRegistrations();

            builder.Services.AddServiceRegistrations();

            builder.Services.AddControllers().AddOData(options =>
            {
                options.Select().Filter().Expand().OrderBy().Count().SetMaxTop(null);
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }


}

