using $safeprojectname$.Modules.Api.Interfaces.Services;
using $safeprojectname$.Modules.Api.Services;
using $safeprojectname$.Modules.ModuleExtensions;

namespace $safeprojectname$.Modules.Api
{
	public class ApiModule : IModule
	{
		public IServiceCollection RegisterModules(IServiceCollection services)
		{
			services.AddScoped<IApiService, ApiService>();

			return services;
		}

		public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
		{
			return endpoints;
		}
	}
}