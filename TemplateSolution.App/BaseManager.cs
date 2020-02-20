using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateSolution.Repository.Core;
using TemplateSolution.Repository.Interface;

namespace TemplateSolution.App
{
    public class BaseManager<T> where T : BaseEntity
    {
       
        protected IRepository<T> Repository;

        
        protected IUnitWork UnitWork;

        public BaseManager(IUnitWork unitWork, IRepository<T> repository)
        {
            UnitWork = unitWork;
            Repository = repository;
        }
       
        public void Delete(int[] ids)
        {
            Repository.Delete(u => ids.Contains(u.Id));
        }

        public T Get(int id)
        {
            return Repository.FindSingle(u => u.Id == id);
        }
    }
}
