using Microsoft.Extensions.Configuration;
using MiMall.Common.Enum;
using MiMall.Common.IFactory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Common.Redis
{
    public class RedisCacheString : RedisCacheManager
    {
        public RedisCacheString(IRedisFactory redisFactory)
            : base(redisFactory)
        {
        }
    }
}
