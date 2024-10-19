﻿using Autofac;

namespace Yarnique.Common.Infrastructure
{
    public class ServiceProviderWrapper : IServiceProvider
    {
        private readonly ILifetimeScope lifeTimeScope;

        public ServiceProviderWrapper(ILifetimeScope lifeTimeScope)
        {
            this.lifeTimeScope = lifeTimeScope;
        }

#nullable enable
        public object? GetService(Type serviceType) => this.lifeTimeScope.ResolveOptional(serviceType);
    }
}
