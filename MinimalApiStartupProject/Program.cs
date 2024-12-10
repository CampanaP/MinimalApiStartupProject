using Microsoft.Extensions.DependencyInjection;
using MinimalApiStartupProject.Infrastructures.Extensions;
using Serilog;

namespace MinimalApiStartupProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, config) =>
                config.ReadFrom.Configuration(context.Configuration));

            builder.Services.RegisterServicesFromAttributes();
            builder.Services.RegisterModules();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpClient();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            app.UseSerilogRequestLogging();

            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("An error was found in the request.");
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapEndpoints();
            app.Run();
        }
    }
}