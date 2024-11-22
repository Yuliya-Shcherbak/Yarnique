using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using MediatR;
using MediatR.Pipeline;
using System.Reflection;
using Yarnique.Common.Infrastructure;
using Yarnique.Common.Application.Configuration.Commands;
using Yarnique.Common.Application.Configuration.Queries;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Mediator
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceProviderWrapper>()
                .As<IServiceProvider>()
                .InstancePerDependency()
                .IfNotRegistered(typeof(IServiceProvider));

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var mediatorOpenTypes = new[]
            {
                typeof(IQueryHandler<,>),
                typeof(INotificationHandler<>),
                typeof(IRequestPreProcessor<>),
                typeof(IStreamRequestHandler<,>),
                typeof(IRequestPostProcessor<,>),
                typeof(IRequestExceptionHandler<,,>),
                typeof(IRequestExceptionAction<,>),
                typeof(ICommandHandler<>),
                typeof(ICommandHandler<,>),
            };
            builder.RegisterSource(new ScopedContravariantRegistrationSource(
                mediatorOpenTypes));
            foreach (var mediatorOpenType in mediatorOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(Assemblies.Application, ThisAssembly)
                    .AsClosedTypesOf(mediatorOpenType)
                    .AsImplementedInterfaces()
                    .FindConstructorsWith(new AllConstructorFinder());
            }

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }

        private class ScopedContravariantRegistrationSource : IRegistrationSource
        {
            private readonly ContravariantRegistrationSource _source = new();
            private readonly List<Type> _types = new();

            public ScopedContravariantRegistrationSource(params Type[] types)
            {
                ArgumentNullException.ThrowIfNull(types);

                if (!types.All(x => x.IsGenericTypeDefinition))
                {
                    throw new ArgumentException("Supplied types should be generic type definitions");
                }

                _types.AddRange(types);
            }

            public IEnumerable<IComponentRegistration> RegistrationsFor(
                Service service,
                Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
            {
                var components = _source.RegistrationsFor(service, registrationAccessor);
                foreach (var c in components)
                {
                    var defs = c.Target.Services
                        .OfType<TypedService>()
                        .Select(x => x.ServiceType.GetGenericTypeDefinition());

                    if (defs.Any(_types.Contains))
                    {
                        yield return c;
                    }
                }
            }

            public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
        }
    }
}
