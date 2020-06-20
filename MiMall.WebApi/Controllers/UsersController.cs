using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MiMall.IService.IServices;
using MiMall.Model.Entity;
using MiMall.Model.Models;
using MiMall.WebApi.Auth;

namespace MiMall.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("any")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;
        private readonly IUserRoleService _userRoleService;
        public UsersController(ILogger<UsersController> logger, IUsersService usersService, IConfiguration configuration,
            IUserRoleService userRoleService)
        {
            _logger = logger;
            _usersService = usersService;
            _configuration = configuration;
            _userRoleService = userRoleService;
        }

        [Authorize("SystemOrAdmin")]
        [HttpGet]
        public TModel<Users> GetModel(int id)
        {

            return new TModel<Users>()
            {
                status = 0,
                message = "success",
                Data = _usersService.Find(id).Result
            };

        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        [HttpPost]
        public TModel<dynamic> Login(string username, string password)
        {

            var user = _usersService.FirstOrDefault(u => u.UserName == username && u.Userpwd == password).Result;


            if (user == null)
            {
                return new TModel<dynamic>()
                {
                    status = 20,
                    message = "用户名或密码错误",
                    Data = user
                };
            }
            else
            {
                string reoleNmae = _userRoleService.Find(user.RoleId).Result.RoleName;

                //声明参数
                Claim[] claims =
                {
                    new Claim(JwtRegisteredClaimNames.AuthTime,DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss fff")),
                    new Claim(JwtRegisteredClaimNames.Sub,user.UserId.ToString()),
                    new Claim(ClaimTypes.Role,reoleNmae)
                };
                //读取配置文件
                JwtAuthModel model = _configuration.GetSection("JwtAuthModel").Get<JwtAuthModel>();
                //创建token对象
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: model.Issuer,//发行人
                    audience: model.Audience,//订阅人
                    claims: claims,//声明参数
                    expires: DateTime.Now.AddSeconds(model.AccessExpiration),//过期时间
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(model.SigningKey)),//密钥
                        SecurityAlgorithms.HmacSha256)//加密方式
                    );
                //生成token字符串
                string jwtString = new JwtSecurityTokenHandler().WriteToken(token);

                //获取刷新token
                string refTokens = GenerateRefreshToken();

                //写入cookie
                Response.Cookies.Append("access_token", jwtString, new CookieOptions()
                {
                    Expires = DateTime.Now.AddMinutes(30)
                });//访问token
                Response.Cookies.Append("refresh_token", refTokens, new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(7)//过期时间 7天
                });//刷新token 

                //获取token
                string cookie = Request.Cookies["access_token"];

                return new TModel<dynamic>()
                {
                    status = 0,
                    message = "success",
                    Data = new
                    {
                        user
                        //,jwtString
                    }
                };
            }

        }

        /// <summary>
        /// 刷新token
        /// </summary>
        [HttpPut]
        public void RefreshToken(string access_token)
        {

        }

        /// <summary>
        /// 生成刷新Token
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize("everyone")]
        public TModel<Users> GetUserInfo()
        {
            //获取token内容的3中方法
            string token = Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(token))
            {
                return new TModel<Users>()
                {
                    status = 10,
                    message = "token过期",
                    Data = null
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

            Users user = _usersService.Find(int.Parse(userId)).Result;

            return new TModel<Users>()
            {
                status = 0,
                message = "success",
                Data = user
            };

        }


    }
}
