using Microsoft.EntityFrameworkCore;
using MiMall.IRepository.IRepositorys;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Repository.Repositorys
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context)
            : base(context)
        {
        }
    }
}
