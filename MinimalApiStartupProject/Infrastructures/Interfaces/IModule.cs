namespace MinimalApiStartupProject.Infrastructures.ModuleExtensions
{
    public interface IModule
    {
        /// <summary>
        /// Method that register IModule classes
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        IServiceCollection RegisterModules(IServiceCollection builder);

        /// <summary>
        /// Map endpoints for module
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}