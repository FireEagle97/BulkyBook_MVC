﻿using BulkyBook.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Repository.IRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext _db;

        internal DbSet<T> dbSet;

        public Repository(ApplicationContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            _db.Product.Include(u => u.Category);
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        //includeProp - "Category, CoverType"
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach(var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;         
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }
       

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entites)
        {
            dbSet.RemoveRange(entites);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}
