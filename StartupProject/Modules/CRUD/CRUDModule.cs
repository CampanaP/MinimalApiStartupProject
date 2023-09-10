using $safeprojectname$.Modules.CRUD.Interfaces.Services;
using $safeprojectname$.Modules.CRUD.Services;
using $safeprojectname$.Modules.ModuleExtensions;

namespace $safeprojectname$.Modules.CRUD
{
	public class CRUDModule : IModule
	{
		public IServiceCollection RegisterModules(IServiceCollection services)
		{
			services.AddScoped<ICRUDService, CRUDService>();

			return services;
		}

		public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
		{
			return endpoints;
		}
	}
}