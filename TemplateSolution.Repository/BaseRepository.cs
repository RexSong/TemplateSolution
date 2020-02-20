using System;
using System.Linq;
using System.Linq.Expressions;
using TemplateSolution.Repository.Core;
using TemplateSolution.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace TemplateSolution.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private TemplateDBContext _context;

        public BaseRepository(TemplateDBContext context)
        {
            _context = context;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> exp = null)
        {
            return Filter(exp);
        }

        public bool IsExist(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().Any(exp);
        }

        public T FindSingle(Expression<Func<T, bool>> exp)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(exp);
        }

        public IQueryable<T> Find(int pageindex, int pagesize, string orderby = "", Expression<Func<T, bool>> exp = null)
        {
            if (pageindex < 1) pageindex = 1;
            if (string.IsNullOrEmpty(orderby))
                orderby = "Id descending";

            //return Filter(exp).OrderBy(orderby).Skip(pagesize * (pageindex - 1)).Take(pagesize);
            return Filter(exp).Skip(pagesize * (pageindex - 1)).Take(pagesize);
        }

        public int GetCount(Expression<Func<T, bool>> exp = null)
        {
            return Filter(exp).Count();
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            Save();
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void BatchAdd(T[] entities)
        {
            _context.Set<T>().AddRange(entities);
            Save();
        }

        public void Update(T entity)
        {
            var entry = this._context.Entry(entity);
            entry.State = EntityState.Modified;

            //如果数据没有发生变化
            if (!this._context.ChangeTracker.HasChanges())
            {
                return;
            }

            Save();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            Save();
        }

        public void Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity)
        {
            _context.Set<T>().Where(where).Update(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> exp)
        {
            _context.Set<T>().Where(exp).Delete();
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

        private IQueryable<T> Filter(Expression<Func<T, bool>> exp)
        {
            var dbSet = _context.Set<T>().AsNoTracking().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
        }

        public int ExecuteSql(string sql)
        {
            return _context.Database.ExecuteSqlCommand(sql);
        }
    }
}
