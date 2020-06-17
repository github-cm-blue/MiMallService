using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiMall.IService.IServices;
using MiMall.Model.Entity;
using MiMall.Model.Models;

namespace MiMall.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("any")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;
        public UsersController(ILogger<UsersController> logger, IUsersService usersService)
        {
            _logger = logger;
            _usersService = usersService;
        }

        private static int skip = 0;
        [HttpGet]
        public TableModel<Users> GetList(int pageSize)
        {
            if (skip > 0)
            {
                skip += pageSize;
            }
            var list = _usersService.GetPage<int>(u => u.UserId > 0, o => o.UserId, true, skip, pageSize);

            return new TableModel<Users>()
            {
                status = 0,
                message = string.Empty,
                Data = list
            };
        }


    }
}
