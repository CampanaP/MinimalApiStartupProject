using MinimalApiStartupProject.Infrastructures.ModuleExtensions;

namespace MinimalApiStartupProject.Infrastructures.Extensions
{
    public static class ModuleExtension
    {
        static readonly List<IModule> registeredModules = new List<IModule>();

        /// <summary>
        /// Private method to get all IModule classes 
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<IModule> DiscoverModules()
        {
            return typeof(IModule).Assembly
                .GetTypes()
                .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
                .Select(Activator.CreateInstance)
                .Cast<IModule>();
        }

        /// <summary>
        /// Method that register IModule classes
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterModules(this IServiceCollection services)
        {
            IEnumerable<IModule> modules = DiscoverModules();

            foreach (IModule module in modules)
            {
                module.RegisterModules(services);
                registeredModules.Add(module);
            }

            return services;
        }

        /// <summary>
        /// Map endpoints for module
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication MapEndpoints(this WebApplication app)
        {
            foreach (IModule module in registeredModules)
            {
                module.MapEndpoints(app);

            }
            return app;
        }
    }
}