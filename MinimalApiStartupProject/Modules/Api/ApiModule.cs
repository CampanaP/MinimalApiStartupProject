using MinimalApiStartupProject.Infrastructures.ModuleExtensions;

namespace MinimalApiStartupProject.Modules.Api
{
    public class ApiModule : IModule
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