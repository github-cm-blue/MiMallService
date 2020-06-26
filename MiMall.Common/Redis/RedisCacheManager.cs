using MiMall.Common.IFactory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Common.Redis
{
    public abstract class RedisCacheManager : IRedisCacheManager
    {
        private readonly IRedisFactory _redisFactory;

        public RedisCacheManager(IRedisFactory redisFactory)
        {
            _redisFactory = redisFactory;
        }

        public bool Set<T>(string key, T value, TimeSpan? expiry = null, int dbIndex = 0) where T : class
        {
            string Tvalue = value as string;
            return _redisFactory.getRedisConnection().GetDatabase(dbIndex).StringSet(key, Tvalue, expiry);
        }

        public bool Set<T>(KeyValuePair<RedisKey, T>[] kv, int dbIndex = 0) where T : class
        {
            KeyValuePair<RedisKey, RedisValue>[] keyValue = kv as KeyValuePair<RedisKey, RedisValue>[];
            return _redisFactory.getRedisConnection().GetDatabase(dbIndex).StringSet(keyValue);
        }


        public T Get<T>(string key, int dbIndex = 0) where T : class
        {
            T result = _redisFactory.getRedisConnection().GetDatabase(dbIndex).StringGet(key) as T;
            return result;
        }

        public long GetLength(string key, int dbIndex = 0)
        {
            return _redisFactory.getRedisConnection().GetDatabase(dbIndex).StringLength(key);
        }


    }
}
