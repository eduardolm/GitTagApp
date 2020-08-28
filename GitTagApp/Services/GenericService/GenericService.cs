using System.Collections.Generic;
using Castle.Core.Internal;
using GitTagApp.Interfaces;

namespace GitTagApp.Services.GenericService
{
    public class GenericService<T> : IGenericService<T> where T : class, IBaseEntity
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }
        
        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(int id)
        {
            if (id <= 0) return null;
            return _repository.GetById(id);
        }

        public T Create(T entity)
        {
            _repository.Create(entity);
            return _repository.GetById(entity.Id);
        }

        public T Update(T entity)
        {
            if (entity.Id.ToString().IsNullOrEmpty() || entity.Id <= 0 || _repository.GetById(entity.Id) == null) return null;
            _repository.Update(entity);
            return _repository.GetById(entity.Id);
        }

        public IEnumerable<T> Delete(int id)
        {
            if (id.ToString().IsNullOrEmpty() || id <= 0 || _repository.GetById(id) == null) return null;
            _repository.Delete(id);
            return _repository.GetAll();
        }
        
        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}