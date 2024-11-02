using Autofac;
using MediatR;
using Yarnique.Common.Application.Events;
using Yarnique.Common.Infrastructure.DomainEventsDispatching;
using Yarnique.Common.Infrastructure.UnitOfWork;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Configuration.Processing
{
    internal class ProcessingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DomainEventsDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainNotificationsMapper>()
                .As<IDomainNotificationsMapper>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DomainEventsAccessor>()
                .As<IDomainEventsAccessor>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(
                typeof(UnitOfWorkCommandHandlerDecorator<>),
                typeof(IRequestHandler<>));

            builder.RegisterGenericDecorator(
                typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
                typeof(IRequestHandler<,>));

            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerDecorator<>),
                typeof(IRequestHandler<>));

            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerWithResultDecorator<,>),
                typeof(IRequestHandler<,>));

            builder.RegisterGenericDecorator(
                typeof(ValidationCommandHandlerDecorator<>),
                typeof(IRequestHandler<>));

            builder.RegisterGenericDecorator(
                typeof(ValidationCommandHandlerWithResultDecorator<,>),
                typeof(IRequestHandler<,>));

            builder.RegisterGenericDecorator(
                typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
                typeof(INotificationHandler<>));

            builder.RegisterAssemblyTypes(Assemblies.Application)
                .AsClosedTypesOf(typeof(IDomainEventNotification<>))
                .InstancePerDependency()
                .FindConstructorsWith(new AllConstructorFinder());
        }
    }
}
