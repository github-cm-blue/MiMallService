using Microsoft.EntityFrameworkCore;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Repository.Repositorys
{
    public class ShoppingCartRepository : BaseRepository<ShoppingCart>
    {
        public ShoppingCartRepository(DbContext context)
            : base(context)
        {
        }
    }
}
