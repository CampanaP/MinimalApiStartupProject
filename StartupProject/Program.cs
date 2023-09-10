using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using $safeprojectname$.Modules.ModuleExtensions;
using System.Text.Json;

namespace $safeprojectname$
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.File("Logs/log.txt",
					rollingInterval: RollingInterval.Day,
					outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
				.CreateLogger();

			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			builder.Host.UseSerilog();
			builder.Services.RegisterModules();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddHttpClient();
			builder.Services.AddSwaggerGen();

			WebApplication app = builder.Build();

			if (builder.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseSerilogRequestLogging();

			app.UseExceptionHandler(exceptionHandlerApp =>
			{
				exceptionHandlerApp.Run(async context =>
				{
					IExceptionHandlerPathFeature? exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
					if (exceptionHandlerPathFeature is not null)
					{
						Log.Error($"{JsonSerializer.Serialize(exceptionHandlerPathFeature.Error)}");
					}

					context.Response.StatusCode = StatusCodes.Status500InternalServerError;
					await context.Response.WriteAsync("An error was found in the request.");
				});
			});

			app.MapEndpoints();
			app.Run();
		}
	}
}