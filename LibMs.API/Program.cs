
using System.Text.Json.Serialization;
using LibMs.API;
using LibMs.Persistance;
using LibMS.Business;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;


namespace LibMs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AppSettings appSettings = new AppSettings();
            builder.Configuration.Bind(appSettings);
            // AppSettings appSettings = builder.Configuration.Get<AppSettings>();
            builder.Services.AddSingleton(appSettings);

            builder.Services.AddDbContext<LibMSContext>((options) =>
            {
                options
                    .UseNpgsql(appSettings.Database.ConnectionString);
            });

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

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // Use ReferenceHandler.Preserve to handle circular references.
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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

