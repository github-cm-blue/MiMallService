using Microsoft.EntityFrameworkCore;
using MiMall.IRepository.IRepositorys;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Repository.Repositorys
{
    public class UsersRepostiory : BaseRepository<Users>, IUsersRepository
    {
        public UsersRepostiory(DbContext context)
            : base(context)
        {
        }
    }
}
