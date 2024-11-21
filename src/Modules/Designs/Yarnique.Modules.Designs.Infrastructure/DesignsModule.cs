using Autofac;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using Yarnique.Common.Application.Contracts;
using Yarnique.Common.Infrastructure.Helpers;
using Yarnique.Modules.Designs.Application.Contracts;
using Yarnique.Modules.Designs.Infrastructure.Configuration;
using Yarnique.Modules.Designs.Infrastructure.Configuration.Processing;

namespace Yarnique.Modules.Designs.Infrastructure
{
    public class DesignsModule : IDesignsModule
    {
        private static readonly IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private readonly ConcurrentDictionary<string, string> _keys = new();
        // TODO: pass it as DI
        private readonly TimeSpan _absoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
        private readonly TimeSpan _slidingExpiration = TimeSpan.FromMinutes(10);


        public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
        {
            var cacheKeys = CacheHelper.GetCacheableKeyName(command);
            InvalidateCache(cacheKeys);
            return await CommandsExecutor.Execute(command);
        }

        public async Task ExecuteCommandAsync(ICommand command)
        {
            var cacheKeys = CacheHelper.GetCacheableKeyName(command);
            InvalidateCache(cacheKeys);
            await CommandsExecutor.Execute(command);
        }

        public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
        {
            using (var scope = DesignsCompositionRoot.BeginLifetimeScope())
            {
                var mediator = scope.Resolve<IMediator>();
                return await ProcessQueryWithCaching(mediator, query);
            }
        }

        internal async Task<TResult> ProcessQueryWithCaching<TResult>(IMediator mediator, IQuery<TResult> query)
        {
            var cacheKeys = CacheHelper.GetCacheableKeyName(query);
            if (cacheKeys.Length == 0)
                return await mediator.Send(query);

            return await _cache.GetOrCreateAsync(cacheKeys[0], async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _absoluteExpirationRelativeToNow;
                entry.SlidingExpiration = _slidingExpiration;
                _keys.TryAdd(cacheKeys[0], cacheKeys[0]);

                return await mediator.Send(query);
            });
        }

        internal void InvalidateCache(string[] keyPrefixes)
        {
            lock (_keys)
            {
                foreach (var keyPrefix in keyPrefixes)
                    foreach (var key in _keys.Keys.Where(x => x.StartsWith(keyPrefix)).ToList())
                    {
                        _cache.Remove(key);
                        _keys.TryRemove(key, out var _);
                    }
            }
        }
    }
}
