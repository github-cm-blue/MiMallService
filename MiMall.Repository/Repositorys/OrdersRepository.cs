using Microsoft.EntityFrameworkCore;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Repository.Repositorys
{
    public class OrdersRepository : BaseRepository<Orders>
    {
        public OrdersRepository(DbContext context)
            : base(context)
        {
        }
    }
}
