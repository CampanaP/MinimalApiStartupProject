namespace $safeprojectname$.Modules.ModuleExtensions
{
	public static class ModuleExtensions
	{
		static readonly List<IModule> registeredModules = new List<IModule>();

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

		public static WebApplication MapEndpoints(this WebApplication app)
		{
			foreach (IModule module in registeredModules)
			{
				module.MapEndpoints(app);

			}
			return app;
		}

		private static IEnumerable<IModule> DiscoverModules()
		{
			return typeof(IModule).Assembly
			.GetTypes()
				.Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
				.Select(Activator.CreateInstance)
				.Cast<IModule>();
		}
	}
}