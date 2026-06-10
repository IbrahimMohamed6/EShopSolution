
using BuildingBlocks.Behaviours;
using BuildingBlocks.Exeptions;
using Carter;
using FluentValidation;
using JasperFx;
using Marten;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Catalog.Ai
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCarter();
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblyContaining<Program>();
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
              });
            builder.Services.AddMarten(options =>
            {
                options.Connection(builder.Configuration.GetConnectionString("PostgreeConnection")!);
                options.AutoCreateSchemaObjects = AutoCreate.All;
            }).UseLightweightSessions();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            var app = builder.Build();
            app.MapCarter();


            
            app.UseExceptionHandler();
            app.Run();
        }
    }
}
