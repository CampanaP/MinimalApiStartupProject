using MinimalApiStartupProject.Infrastructures.ModuleExtensions;

namespace MinimalApiStartupProject.Modules.CRUD
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