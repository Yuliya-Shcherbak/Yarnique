using Autofac;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Configuration
{
    internal static class UsersManagementCompositionRoot
    {
        private static IContainer _container;

        public static void SetContainer(IContainer container)
        {
            _container = container;
        }

        public static ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }
    }
}
