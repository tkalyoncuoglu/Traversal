﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IGenericDal<T> where T:class
    {
        void Insert(T t);
        void Update(T t);
        void Delete(T t);
        List<T> GetList();
        List<T> GetList(Expression<Func<T, bool>> predicate);
        T GetByID(int id);

        IGenericDal<T> Include(string[] includes);
    }
}
