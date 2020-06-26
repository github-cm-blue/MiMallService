using MiMall.Common.Enum;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiMall.Common.IFactory
{
    public interface IRedisFactory
    {
        ConnectionMultiplexer getRedisConnection();
    }
}
