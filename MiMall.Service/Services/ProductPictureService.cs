using MiMall.IRepository;
using MiMall.IRepository.IRepositorys;
using MiMall.IService.IServices;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Service.Services
{
    public class ProductPictureService : BaseService<ProductPicture>, IProductPictureService
    {
        public ProductPictureService(IBaseRepository<ProductPicture> baseRepository)
            : base(baseRepository)
        {
        }
    }
}
