using MinimalApiStartupProject.Infrastructures.ModuleExtensions;

namespace MinimalApiStartupProject.Modules.Email
{
    public class EmailModule : IModule
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