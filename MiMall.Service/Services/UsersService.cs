﻿using MiMall.IRepository;
using MiMall.IService.IServices;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Service.Services
{
    public class UsersService : BaseService<Users>, IUsersService
    {
        public UsersService(IBaseRepository<Users> baseRepository)
            : base(baseRepository)
        {
        }
    }
}
