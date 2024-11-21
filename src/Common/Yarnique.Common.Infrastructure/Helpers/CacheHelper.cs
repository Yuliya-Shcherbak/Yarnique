using Yarnique.Common.Application.Configuration.Attributes;
using Yarnique.Common.Application.Contracts;

namespace Yarnique.Common.Infrastructure.Helpers
{
    public static class CacheHelper
    {
        public static string[] GetCacheableKeyName<TResult>(IQuery<TResult> query)
        {
            var attributes = query.GetType().GetCustomAttributes(typeof(CacheableEntityAttribute), true);
            if (attributes.Any())
            {
                var entitiesNames = (attributes.First() as CacheableEntityAttribute).GetEntities();

                if (entitiesNames.Length > 1)
                    throw new Exception("The query should have a reference to the root entity only for caching");

                if (query is QueryBaseWithPaging<TResult>)
                {
                    var (pageNumber, pageSize) = (QueryBaseWithPaging<TResult>)query;
                    return [$"{entitiesNames[0]}|{pageNumber}|{pageSize}"];
                }

                return entitiesNames;
            }

            return Array.Empty<string>();
        }

        public static string[] GetCacheableKeyName<TResult>(ICommand<TResult> command)
        {
            var attributes = command.GetType().GetCustomAttributes(typeof(CacheableEntityAttribute), true);
            if (attributes.Any())
            {
                return (attributes.First() as CacheableEntityAttribute).GetEntities();
            }

            return Array.Empty<string>();
        }

        public static string[] GetCacheableKeyName(ICommand command)
        {
            var attributes = command.GetType().GetCustomAttributes(typeof(CacheableEntityAttribute), true);
            if (attributes.Any())
            {
                return (attributes.First() as CacheableEntityAttribute).GetEntities();
            }

            return Array.Empty<string>();
        }
    }
}
