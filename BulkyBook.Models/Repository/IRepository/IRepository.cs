﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked= false);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null,string? includeProperties = null);
        void Add(T item);
        void Update(T item); 
        void Remove(T item);
        void RemoveRange(IEnumerable<T> items);
    }
}
