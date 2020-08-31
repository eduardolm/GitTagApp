using System;
using System.Collections.Generic;
using System.Linq;
using GitTagApp.Entities;
using GitTagApp.Repositories.Context;
using GitTagApp.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace GitTagApp.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IBaseEntity
    {
        private readonly MainContext _dbContext;

        public GenericRepository(MainContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T GetById(long id)
        {
            return _dbContext.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public void Create(T entity)
        {
            var checkId = _dbContext.Set<T>().Find(entity.Id);
            if (checkId != null) return;
            
            DetachLocal(_ => _.Id == entity.Id);
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            DetachLocal(_ => _.Id == entity.Id);
            _dbContext.Set<T>().Update(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(long id)
        {
            var entity = _dbContext.Set<T>().FirstOrDefault(x => x.Id == id);
            _dbContext.Remove(entity);
            _dbContext.SaveChanges();
        }
        
        public void Dispose()
        {
           _dbContext.Dispose();
        }
        
        public virtual void DetachLocal(Func<T, bool> predicate)
        {
            var local = _dbContext.Set<T>().Local.Where(predicate).FirstOrDefault();
            if (local != null)
            {
                _dbContext.Entry(local).State = EntityState.Detached;
            }
        }
    }
}