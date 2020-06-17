using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MiMall.IService
{
    public interface IBaseService<T> where T : class, new()
    {
        /// <summary>
        /// 添加单个实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public Task<int> Add(T entity);

        /// <summary>
        /// 添加多个实体
        /// </summary>
        /// <param name="entitys">实体集合</param>
        /// <returns></returns>
        public Task<int> Add(List<T> entitys);

        /// <summary>
        /// 根据id删除实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<int> Delete(int id);

        /// <summary>
        /// 根据id删除多个实体
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        public Task<int> Delete(params int[] ids);

        /// <summary>
        /// 如果不传入prototypes，则默认修改整个实体，反之，只修改某个属性值
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="prototypes">属性</param>
        /// <returns></returns>
        public Task<int> Update(T entity, params string[] prototypes);

        /// <summary>
        /// 根据条件查询符合条件的第一个元素
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public Task<T> FirstOrDefault(Expression<Func<T, bool>> where);

        /// <summary>
        /// 可根据条件查询数据，并可指定排序规则
        /// </summary>
        /// <typeparam name="S">排序字段的类型</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="isAsc">是否为顺序</param>
        /// <returns></returns>
        public Task<List<T>> GetList<S>(Expression<Func<T, bool>> where = null
            , Expression<Func<T, S>> orderBy = null, bool isAsc = true);

        /// <summary>
        /// 查询分页数据，
        /// </summary>
        /// <typeparam name="S">排序字段的类型</typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序字段</param>
        /// <param name="isAsc">是否为顺序</param>
        /// <param name="skip">要跳过几条</param>
        /// <param name="take">要查询几条</param>
        /// <returns></returns>
        public Task<List<T>> GetPage<S>(Expression<Func<T, bool>> where, Expression<Func<T, S>> orderBy
            , bool isAsc, int skip, int take);

        /// <summary>
        /// 根据id查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<T> Find(int id);

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public Task<int> Commit();

    }
}
