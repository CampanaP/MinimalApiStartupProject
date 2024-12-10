using MinimalApiStartupProject.Infrastructures.Attributes;
using Scrutor;

namespace MinimalApiStartupProject.Infrastructures.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Method to register services with Lifetime attributes
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterServicesFromAttributes(this IServiceCollection services)
        {
            return services
                .Scan(scan => scan
                    .FromApplicationDependencies()

                    //Transient classes
                    .AddClasses(@class => @class.WithAttribute<TransientLifetimeAttribute>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime()

                    //Scoped classes
                    .AddClasses(@class => @class.WithAttribute<ScopedLifetimeAttribute>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()

                    //Singleton classes
                    .AddClasses(@class => @class.WithAttribute<SingletonLifetimeAttribute>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime());
        }
    }
}