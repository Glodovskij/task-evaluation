using images_viewer.DAL.EF;
using images_viewer.DAL.Entities;
using images_viewer.DAL.Interfaces;
using images_viewer.DAL.Repositories;
using System;
using System.Collections.Generic;

namespace images_viewer.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDictionary<Type, object> _repositories;
        private readonly PicDbContext _dbContext;

        public UnitOfWork(PicDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<Type, object>();

            Register(new Repository<Picture>(dbContext));
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Register<T>(Repository<T> repository) where T : BaseEntity
        {
            _repositories.Add(typeof(Repository<T>), repository);
        }

        public Repository<T> GetRepository<T>() where T : BaseEntity
        {
            if(_repositories.ContainsKey(typeof(Repository<T>)))
            {
                return _repositories[typeof(Repository<T>)] as Repository<T>;
            }
            return null;
        }
    }
}
