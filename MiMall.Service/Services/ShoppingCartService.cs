using MiMall.IRepository;
using MiMall.IService.IServices;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Service.Services
{
    public class ShoppingCartService : BaseService<ShoppingCart>, IShoppingCartService
    {
        public ShoppingCartService(IBaseRepository<ShoppingCart> baseRepository)
            : base(baseRepository)
        {
        }
    }
}
