using $safeprojectname$.Modules.Email.Interfaces.Services;
using $safeprojectname$.Modules.Email.Services;
using $safeprojectname$.Modules.ModuleExtensions;

namespace $safeprojectname$.Modules.Email
{
	public class EmailModule : IModule
	{
		public IServiceCollection RegisterModules(IServiceCollection services)
		{
			services.AddScoped<IEmailService, EmailService>();

			return services;
		}

		public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
		{
			return endpoints;
		}
	}
}