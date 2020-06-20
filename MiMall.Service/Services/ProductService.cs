using MiMall.IRepository;
using MiMall.IService.IServices;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Service.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(IBaseRepository<Product> baseRepository)
            : base(baseRepository)
        {
        }
    }
}
