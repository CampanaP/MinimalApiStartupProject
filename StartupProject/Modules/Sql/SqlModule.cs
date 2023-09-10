using $safeprojectname$.Modules.ModuleExtensions;
using $safeprojectname$.Modules.Sql.Interfaces.Repositories;
using $safeprojectname$.Modules.Sql.Repositories;

namespace $safeprojectname$.Modules.Sql
{
	public class SqlModule : IModule
	{
		public IServiceCollection RegisterModules(IServiceCollection services)
		{
			services.AddScoped<ISqlRepository, SqlRepository>();

			return services;
		}

		public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
		{
			return endpoints;
		}
	}
}