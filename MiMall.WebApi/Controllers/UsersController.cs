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
    [Route("api/[controller]")]
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

        [HttpGet]
        public TModel<Users> GetModel(int id)
        {

            return new TModel<Users>()
            {
                status = 0,
                message = "success",
                Data = _usersService.Find(id)
            };

        }


    }
}
