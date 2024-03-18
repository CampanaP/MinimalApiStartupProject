using $safeprojectname$.Infrastructures.ServiceExtensions.Attributes;
using Scrutor;

namespace $safeprojectname$.Infrastructures.ServiceExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddFromAttributes(this IServiceCollection services)
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