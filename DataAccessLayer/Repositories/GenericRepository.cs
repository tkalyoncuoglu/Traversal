using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        protected Context c;

        protected IQueryable<T> query;

        public GenericRepository(Context c) 
        { 
            this.c = c;

            query = c.Set<T>().AsQueryable();
        }
        public void Delete(T t)
        {
            c.Remove(t);
            c.SaveChanges();
        }

        public T GetByID(int id)
        {
            return c.Set<T>().Find(id);
        }

        public List<T> GetList()
        {
            return query.ToList();
        }

        public List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            return query.Where(predicate).ToList();
        }

        public IGenericDal<T> Include(string[] includes)
        {
            foreach(var include in includes) 
            { 
                query = query.Include(include);
            }

            return this;
        }

        public void Insert(T t)
        {
            c.Add(t);
            c.SaveChanges();
        }

        public void Update(T t)
        {
            c.Update(t);
            c.SaveChanges();
        }
    }
}
