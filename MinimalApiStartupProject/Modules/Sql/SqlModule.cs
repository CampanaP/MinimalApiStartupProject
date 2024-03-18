using $safeprojectname$.Infrastructures.ModuleExtensions;

namespace $safeprojectname$.Modules.Sql
{
    public class SqlModule : IModule
    {
        public IServiceCollection RegisterModules(IServiceCollection services)
        {
            return services;
        }

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            return endpoints;
        }
    }
}