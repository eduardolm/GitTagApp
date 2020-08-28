using System;
using System.Collections.Generic;

namespace GitTagApp.Interfaces
{
    public interface IGenericService<T> : IDisposable where T : class, IBaseEntity
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Create(T entity);
        T Update(T entity);
        IEnumerable<T> Delete(int id);
    }
}