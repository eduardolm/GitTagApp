using System;
using System.Collections.Generic;

namespace GitTagApp.Interfaces
{
    public interface IGenericService<T> : IDisposable where T : class, IBaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        string Create(T entity);
        string Update(T entity);
        string Delete(long id);
    }
}