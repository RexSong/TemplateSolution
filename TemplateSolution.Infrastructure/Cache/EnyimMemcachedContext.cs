using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateSolution.Infrastructure.Cache
{
    public class EnyimMemcachedContext : ICacheContext
    {
        private IMemcachedClient _memcachedClient;

        public EnyimMemcachedContext(IMemcachedClient client)
        {
            _memcachedClient = client;
        }

        public T Get<T>(string key)
        {
            return _memcachedClient.Get<T>(key);
        }

        public bool Set<T>(string key, T t, DateTime expire)
        {
            return _memcachedClient.Store(StoreMode.Set, key, t, expire);
        }

        public bool Remove(string key)
        {
            return _memcachedClient.Remove(key);
        }
    }
}
