using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MiMall.Common.Redis
{
    public class RedisConfig
    {
        [JsonPropertyName("masterServer")]
        public string MasterServer { get; set; }

        [JsonPropertyName("slaveServer")]
        public string[] SlaveServer { get; set; }



    }
}
