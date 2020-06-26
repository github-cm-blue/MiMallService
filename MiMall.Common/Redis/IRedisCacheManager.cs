using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Common.Redis
{
    public interface IRedisCacheManager
    {

        public bool Set<T>(string key, T value, TimeSpan? expiry = null, int dbIndex = 0) where T : class;


        public bool Set<T>(KeyValuePair<RedisKey, T>[] kv, int dbIndex = 0) where T : class;


        public T Get<T>(string key, int dbIndex = 0) where T : class;


        public long GetLength(string key, int dbIndex = 0);

    }
}
