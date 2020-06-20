using Microsoft.EntityFrameworkCore;
using MiMall.IRepository.IRepositorys;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Repository.Repositorys
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context)
            : base(context)
        {
        }
    }
}
