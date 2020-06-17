using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiMall.WebApi.AuthHelper
{
    public class JwtModel
    {
        /// <summary>
        /// 密钥
        /// </summary>
        [JsonPropertyName("signingKey")]
        public string SigningKey { get; set; }
        /// <summary>
        /// 发行人
        /// </summary>
        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅人
        /// </summary>
        [JsonPropertyName("audience")]
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        [JsonPropertyName("accessExpiration")]
        public int AccessExpiration { get; set; }
        /// <summary>
        /// 刷新时间
        /// </summary>
        [JsonPropertyName("refreshExpiration")]
        public int RefreshExpiration { get; set; }

    }
}
