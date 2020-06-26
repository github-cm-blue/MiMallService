using System;
using System.Collections;
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
using Newtonsoft.Json;

namespace MiMall.WebApi.Controllers
{
    [Route("[controller]/[action]")]
    [EnableCors("any")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;
        private readonly IProductPictureService _productPictureService;
        private readonly ICategoryService _categoryService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService,
            IProductPictureService productPictureService, ICategoryService categoryService)
        {
            _logger = logger;
            _productService = productService;
            _productPictureService = productPictureService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// 导航条列表数据
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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

            //导航条数据：navData

            string json = JsonConvert.SerializeObject(data);



            return new TModel<dynamic>()
            {
                status = 0,
                message = "success",
                Data = data
            };

        }

        /// <summary>
        /// 根据商品id获取商品详情
        /// </summary>
        [HttpGet]
        public TModel<dynamic> getProductInfoById(int id)
        {
            Product product = _productService.Find(id).Result;

            ProductPicture picture = _productPictureService.Find(id).Result;

            return new TModel<dynamic>()
            {
                status = 0,
                message = "success",
                Data = new
                {
                    product,
                    picture
                }
            };

        }

        /// <summary>
        /// 获取分类商品列表
        /// </summary>
        [HttpGet]
        public TModel<dynamic> getCategoryProducts(string cids, int pageSize)
        {

            string[] str = cids.Split('&');
            int[] categoryIds = new int[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                categoryIds[i] = int.Parse(str[i]);
            }


            List<Product> products = new List<Product>();
            List<ProductPicture> pictures = new List<ProductPicture>();


            for (int i = 0; i < categoryIds.Length; i++)
            {
                var pros =
               _productService.GetPage<int>(p => p.CategoryId == categoryIds[i], p => p.ProductSales, true, 0, pageSize).Result;

                products.AddRange(pros);

                List<int> ids = pros.Select(p => p.ProductId).ToList();

                var pics = _productPictureService.GetList<int>(p => ids.Contains(p.ProductId), o => o.ProductId, true).Result;

                pictures.AddRange(pics);
            }

            var data = products.Join(pictures, p => p.ProductId, c => c.ProductId, (p, c) => new
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


        /// <summary>
        /// 获取商品信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public TModel<dynamic> getProductInfo(int productId)
        {
            Product product = _productService.Find(productId).Result;

            ProductPicture picture = _productPictureService.Find(productId).Result;

            return new TModel<dynamic>()
            {
                status = 0,
                message = "success",
                Data = new
                {
                    product,
                    picture
                }
            };
        }



    }
}
