using Microsoft.EntityFrameworkCore;
using MiMall.IRepository.IRepositorys;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Repository.Repositorys
{
    public class ProductPictureRepository : BaseRepository<ProductPicture>, IProductPictureRepository
    {
        public ProductPictureRepository(DbContext context)
            : base(context)
        {
        }
    }
}
