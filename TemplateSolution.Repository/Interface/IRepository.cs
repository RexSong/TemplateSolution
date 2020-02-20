using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TemplateSolution.Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        T FindSingle(Expression<Func<T, bool>> exp = null);
        bool IsExist(Expression<Func<T, bool>> exp);
        IQueryable<T> Find(Expression<Func<T, bool>> exp = null);

        IQueryable<T> Find(int pageindex = 1, int pagesize = 10, string orderby = "",
            Expression<Func<T, bool>> exp = null);

        int GetCount(Expression<Func<T, bool>> exp = null);

        void Add(T entity);

        void BatchAdd(T[] entities);

        void Update(T entity);

        void Delete(T entity);

        void Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity);

        void Delete(Expression<Func<T, bool>> exp);

        void Save();

        int ExecuteSql(string sql);
    }
}
