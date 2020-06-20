using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MiMall.IService.IServices;
using MiMall.Model.Entity;
using MiMall.Model.Models;

namespace MiMall.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("any")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly IProductPictureService _productPictureService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService,
            IProductPictureService productPictureService)
        {
            _logger = logger;
            _productService = productService;
            _productPictureService = productPictureService;
        }

        [HttpGet]
        public TModel<dynamic> GetList(int pageSize)
        {

            //根据销量排序
            List<Product> list = _productService.GetPage<int>
                (p => p.ProductId > 0, o => o.ProductSales, true, 0, pageSize).Result;

            //获取集合中所有的id
            List<int> ids = list.Select(p => p.ProductId).ToList();

            //查询图片
            List<ProductPicture> pictures =
                _productPictureService.GetList<int>(p => ids.Contains(p.ProductId)).Result;

            //联表
            var data = list.Join(pictures, p => p.ProductId, c => c.ProductId, (p, c) => new
            {
                product = p,
                picture = c
            });

            return new TModel<dynamic>()
            {
                status = 0,
                message = "success",
                Data = data
            };

        }

    }
}
