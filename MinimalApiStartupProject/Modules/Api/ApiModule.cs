using $safeprojectname$.Infrastructures.ModuleExtensions;

namespace $safeprojectname$.Modules.Api
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