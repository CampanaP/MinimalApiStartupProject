namespace $safeprojectname$.Modules.ModuleExtensions
{
	public interface IModule
	{
		IServiceCollection RegisterModules(IServiceCollection builder);

		IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
	}
}