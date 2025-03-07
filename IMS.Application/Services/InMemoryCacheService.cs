using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Services
{
    public class InMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _defaultCacheDuration = TimeSpan.FromMinutes(10);

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        // Get a list of entities from cache, or fetch and cache it if not found
        public async Task<List<T>> GetEntitiesCacheAsync<T>(Func<Task<List<T>>> fetchFromDb, string cacheKey)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out List<T> cachedEntities))
            {
                cachedEntities = await fetchFromDb();
                _memoryCache.Set(cacheKey, cachedEntities, _defaultCacheDuration);
            }

            return cachedEntities;
        }

        // Get a single entity from cache, or fetch and cache it if not found
        public async Task<T> GetEntityCacheAsync<T>(Guid entityId, Func<Task<T>> fetchFromDb, string cacheKey)
        {
            if (!_memoryCache.TryGetValue(cacheKey, out T cachedEntity))
            {
                cachedEntity = await fetchFromDb();
                if (cachedEntity != null)
                {
                    _memoryCache.Set(cacheKey, cachedEntity, _defaultCacheDuration);
                }
            }

            return cachedEntity;
        }

        // Remove an entity cache by its ID
        public void RemoveEntityCache<T>(Guid entityId, string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        // Remove all entities cache
        public void RemoveAllEntitiesCache(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
    }
}
