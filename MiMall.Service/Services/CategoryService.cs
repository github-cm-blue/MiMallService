using MiMall.IRepository;
using MiMall.IService.IServices;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Service.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(IBaseRepository<Category> baseRepository)
            : base(baseRepository)
        {
        }
    }
}
