using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrdersService _ordersService;
        public OrdersController(ILogger<OrdersController> logger, IOrdersService ordersService)
        {
            _logger = logger;
            _ordersService = ordersService;
        }

        
    }
}
