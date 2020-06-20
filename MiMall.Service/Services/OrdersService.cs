using MiMall.IRepository;
using MiMall.IService.IServices;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Service.Services
{
    public class OrdersService : BaseService<Orders>, IOrdersService
    {
        public OrdersService(IBaseRepository<Orders> baseRepository)
            : base(baseRepository)
        {
        }
    }
}
