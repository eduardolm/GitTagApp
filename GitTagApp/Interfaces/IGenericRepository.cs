using System;
using System.Collections.Generic;

namespace GitTagApp.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class, IBaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        void Create(T entity);
        void Update(T entity);
        void Delete(long id);
    }
}