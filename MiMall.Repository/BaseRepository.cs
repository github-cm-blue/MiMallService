using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MiMall.IRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MiMall.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        public DbContext context;
        public BaseRepository(DbContext context)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<int> Add(T entity)
        {
            context.Set<T>().Add(entity);

            return await Commit();
        }

        public async Task<int> Add(List<T> entitys)
        {
            context.Set<T>().AddRange(entitys);

            return await Commit();
        }

        public async Task<int> Delete(int id)
        {
            T model = await Find(id);
            context.Set<T>().Remove(model);
            return await Commit();
        }

        public async Task<int> Delete(params int[] ids)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                T model = await Find(ids[i]);
                context.Set<T>().Remove(model);
            }
            return await Commit();
        }

        public Task<int> Update(T entity, params string[] prototypes)
        {
            context.Attach<T>(entity);

            if (prototypes.Length > 0)
            {
                for (int i = 0; i < prototypes.Length; i++)
                {
                    context.Entry<T>(entity).Property(prototypes[i]).IsModified = true;
                }
            }
            else
            {
                context.Entry<T>(entity).State = EntityState.Modified;
            }

            return Commit();
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> where)
        {
            return await context.Set<T>().FirstOrDefaultAsync(where);
        }

        public async Task<List<T>> GetList<S>(Expression<Func<T, bool>> where = null
            , Expression<Func<T, S>> orderBy = null, bool isAsc = true)
        {
            bool b = true;
            List<T> list = new List<T>();
            if (where != null)
            {
                list = await context.Set<T>().Where(where).AsNoTracking().ToListAsync();
                b = false;
            }
            if (orderBy != null)
            {
                if (isAsc)
                {
                    list = await context.Set<T>().Where(where).OrderBy(orderBy).AsNoTracking().ToListAsync();
                }
                else
                {
                    list = await context.Set<T>().Where(where).OrderByDescending(orderBy)
                        .AsNoTracking().ToListAsync();
                }
                b = false;
            }

            if (b)
            {
                list = await context.Set<T>().ToListAsync();
            }

            return list;
        }

        public async Task<List<T>> GetPage<S>(Expression<Func<T, bool>> where, Expression<Func<T, S>> orderBy
            , bool isAsc, int skip, int take)
        {
            List<T> list = new List<T>();

            if (isAsc)
            {
                list = await context.Set<T>().Where(where).OrderBy(orderBy).Skip(skip).Take(take)
                      .AsNoTracking().ToListAsync();
            }
            else
            {
                list = await context.Set<T>().Where(where).OrderByDescending(orderBy).Skip(skip).Take(take)
                     .AsNoTracking().ToListAsync();
            }

            return list;
        }


        public async Task<T> Find(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<int> Commit()
        {
            return await context.SaveChangesAsync();
        }

    }
}
