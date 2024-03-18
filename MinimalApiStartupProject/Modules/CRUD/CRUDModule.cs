using $safeprojectname$.Infrastructures.ModuleExtensions;

namespace $safeprojectname$.Modules.CRUD
{
    public class CRUDModule : IModule
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