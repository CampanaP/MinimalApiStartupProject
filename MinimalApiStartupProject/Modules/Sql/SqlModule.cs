using MinimalApiStartupProject.Infrastructures.ModuleExtensions;

namespace MinimalApiStartupProject.Modules.Sql
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