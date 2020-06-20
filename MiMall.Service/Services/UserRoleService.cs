using MiMall.IRepository;
using MiMall.IService.IServices;
using MiMall.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiMall.Service.Services
{
    public class UserRoleService : BaseService<UserRole>, IUserRoleService
    {
        public UserRoleService(IBaseRepository<UserRole> baseRepository)
            : base(baseRepository)
        {
        }
    }
}
