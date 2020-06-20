using Microsoft.EntityFrameworkCore;
using MiMall.IRepository.IRepositorys;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Repository.Repositorys
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext context)
            : base(context)
        {
        }
    }
}
