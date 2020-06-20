using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiMall.IService.IServices;
using MiMall.Model.Entity;

namespace MiMall.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("any")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(ILogger<ShoppingCartController> logger, IShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _shoppingCartService = shoppingCartService;
        }

        /// <summary>
        /// 获取我的购物车商品数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize("everyone")]
        public TModel<int> GetMyCartCount()
        {
            string token = Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(token))
            {
                return new TModel<int>()
                {
                    status = 10,
                    message = "token过期",
                    Data = 0
                };
            }

            //方式一：JwtSecurityTokenHandler中的ReadJwtToken()方法获取
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(token);
            //string userId = jwtSecurityToken["sub"];
            string userId = string.Empty;
            jwtSecurityToken.Claims.ToList().ForEach(item =>
            {
                if (item.Type == JwtRegisteredClaimNames.Sub)
                {
                    userId = item.Value;
                }
            });

            int cartCount = _shoppingCartService.GetList<int>(s => s.UserId == int.Parse(userId)).Result.Count;

            return new TModel<int>()
            {
                status = 0,
                message = "success",
                Data = cartCount
            };


        }
    }
}
