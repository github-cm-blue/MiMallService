using MiMall.Common.IFactory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Common.Redis
{
    public class RedisCacheHash : IRedisCacheManager
    {
        private readonly IRedisFactory _redisFactory;

        public RedisCacheHash(IRedisFactory redisFactory)
        {
            _redisFactory = redisFactory;

        }
        public T Get<T>(string key, int dbIndex = 0) where T : class
        {
            throw new NotImplementedException();
        }

        public long GetLength(string key, int dbIndex = 0)
        {
            throw new NotImplementedException();
        }

        public bool Set<T>(string key, T value, TimeSpan? expiry = null, int dbIndex = 0) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Set<T>(KeyValuePair<RedisKey, T>[] kv, int dbIndex = 0) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
