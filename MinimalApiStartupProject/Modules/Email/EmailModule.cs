using $safeprojectname$.Infrastructures.ModuleExtensions;

namespace $safeprojectname$.Modules.Email
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