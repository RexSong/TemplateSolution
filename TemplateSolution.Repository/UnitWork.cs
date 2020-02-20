using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TemplateSolution.Repository.Core;
using TemplateSolution.Repository.Interface;
using Z.EntityFramework.Plus;

namespace TemplateSolution.Repository
{
    public class UnitWork : IUnitWork
    {
        private TemplateDBContext _context;

        public UnitWork(TemplateDBContext context)
        {
            _context = context;
        }
        public TemplateDBContext GetDbContext()
        {
            return _context;
        }
     
        public IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp);
        }

        public bool IsExist<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().Any(exp);
        }
       
        public T FindSingle<T>(Expression<Func<T, bool>> exp) where T : class
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(exp);
        }
       
        public IQueryable<T> Find<T>(int pageindex, int pagesize, string orderby = "", Expression<Func<T, bool>> exp = null) where T : class
        {
            if (pageindex < 1) pageindex = 1;
            if (string.IsNullOrEmpty(orderby))
                orderby = "Id descending";

            //return Filter(exp).OrderBy(orderby).Skip(pagesize * (pageindex - 1)).Take(pagesize);
            return Filter(exp).Skip(pagesize * (pageindex - 1)).Take(pagesize);
        }

        public int GetCount<T>(Expression<Func<T, bool>> exp = null) where T : class
        {
            return Filter(exp).Count();
        }

        public void Add<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Add(entity);
        }

        public void BatchAdd<T>(T[] entities) where T : BaseEntity
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Update<T>(T entity) where T : class
        {
            var entry = this._context.Entry(entity);
            entry.State = EntityState.Modified;

            //如果数据没有发生变化
            if (!this._context.ChangeTracker.HasChanges())
            {
                entry.State = EntityState.Unchanged;
            }

        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class
        {
            _context.Set<T>().Where(where).Update(entity);
        }

        public virtual void Delete<T>(Expression<Func<T, bool>> exp) where T : class
        {
            _context.Set<T>().RemoveRange(Filter(exp));
        }

        public void Save()
        {
            //try
            //{
            _context.SaveChanges();
            //}
            //catch (DbEntityValidationException e)
            //{
            //    throw new Exception(e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            //}

        }

        private IQueryable<T> Filter<T>(Expression<Func<T, bool>> exp) where T : class
        {
            var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
        }

        public void ExecuteSql(string sql)
        {
            _context.Database.ExecuteSqlCommand(sql);
        }
    }
}
