using MiMall.IRepository;
using MiMall.IService;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MiMall.Service
{
    public class BaseService<T> : IBaseService<T> where T : class, new()
    {
        public readonly IBaseRepository<T> _baseRepository;
        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public Task<int> Add(T entity)
        {
            return _baseRepository.Add(entity);
        }

        public Task<int> Add(List<T> entitys)
        {
            return _baseRepository.Add(entitys);
        }

        public Task<int> Commit()
        {
            return _baseRepository.Commit();
        }

        public Task<int> Delete(int id)
        {
            return _baseRepository.Delete(id);
        }

        public Task<int> Delete(params int[] ids)
        {
            return _baseRepository.Delete(ids);
        }

        public Task<T> Find(int id)
        {
            return _baseRepository.Find(id);
        }

        public Task<T> FirstOrDefault(Expression<Func<T, bool>> where)
        {
            return _baseRepository.FirstOrDefault(where);
        }

        public Task<List<T>> GetList<S>(Expression<Func<T, bool>> where = null, Expression<Func<T, S>> orderBy = null, bool isAsc = true)
        {
            return _baseRepository.GetList(where, orderBy, isAsc);
        }

        public Task<List<T>> GetPage<S>(Expression<Func<T, bool>> where, Expression<Func<T, S>> orderBy, bool isAsc, int skip, int take)
        {
            return _baseRepository.GetPage<S>(where, orderBy, isAsc, skip, take);
        }

        public Task<int> Update(T entity, params string[] prototypes)
        {
            return _baseRepository.Update(entity, prototypes);
        }
    }
}
