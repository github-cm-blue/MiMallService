using Microsoft.Extensions.Configuration;
using MiMall.Common.Enum;
using MiMall.Common.IFactory;
using MiMall.Common.Redis;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiMall.Common.Factory
{
    public class RedisFactory : IRedisFactory
    {
        private IConfiguration Configuration;
        private static ConnectionMultiplexer connectionMultiplexer;
        public RedisFactory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ConnectionMultiplexer getRedisConnection()
        {
            //需要安装 Microsoft.Extensions.Options.ConfigurationExtensions扩展包 才能使用 Get<>()方法
            RedisConfig[] configs = Configuration.GetSection("Redis:RedisServer").Get<RedisConfig[]>();

            #region 创建全部的redis服务连接
            //Dictionary<ConnectionMultiplexer, List<ConnectionMultiplexer>> dic =
            //    new Dictionary<ConnectionMultiplexer, List<ConnectionMultiplexer>>();
            //for (int i = 0; i < configs.Length; i++)
            //{



            //    ConnectionMultiplexer master = ConnectionMultiplexer.Connect(configs[i].MasterServer);
            //    List<ConnectionMultiplexer> slaves = new List<ConnectionMultiplexer>();
            //    for (int j = 0; j < configs[i].SlaveServer.Length; j++)
            //    {
            //        ConnectionMultiplexer slave = ConnectionMultiplexer.Connect(configs[i].SlaveServer[j]);
            //        slaves.Add(slave);
            //    }
            //    dic.Add(master, slaves);
            //} 
            #endregion

            Random random = new Random((int)DateTime.Now.Ticks);
            RedisConfig redis = configs[random.Next(0, configs.Length)];

            if (connectionMultiplexer == null || !connectionMultiplexer.IsConnected)
            {
                Console.WriteLine("创建新的连接了");
                connectionMultiplexer = ConnectionMultiplexer.Connect(configs[0].MasterServer);
            }


            return connectionMultiplexer;
        }


    }
}
