using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TemplateSolution.Repository.Core;

namespace TemplateSolution.Repository.Interface
{
    public interface IUnitWork
    {
        TemplateDBContext GetDbContext();
        T FindSingle<T>(Expression<Func<T, bool>> exp = null) where T : class;
        bool IsExist<T>(Expression<Func<T, bool>> exp) where T : class;
        IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null) where T : class;

        IQueryable<T> Find<T>(int pageindex = 1, int pagesize = 10, string orderby = "",
            Expression<Func<T, bool>> exp = null) where T : class;

        int GetCount<T>(Expression<Func<T, bool>> exp = null) where T : class;

        void Add<T>(T entity) where T : BaseEntity;

        void BatchAdd<T>(T[] entities) where T : BaseEntity;

        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;

        void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class;

        void Delete<T>(Expression<Func<T, bool>> exp) where T : class;

        void Save();

        void ExecuteSql(string sql);
    }
}
