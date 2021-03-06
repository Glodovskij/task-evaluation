using System;
using System.Collections.Generic;

namespace images_viewer.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity item);
        TEntity FindById(int id);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        void Remove(TEntity item);
        void Update(TEntity item);
        void Commit();
    }
}
