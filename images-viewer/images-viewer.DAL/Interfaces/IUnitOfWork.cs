using images_viewer.DAL.Entities;
using images_viewer.DAL.Repositories;

namespace images_viewer.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        void Commit();

        void Register<T>(Repository<T> repository) where T : BaseEntity;

        Repository<T> GetRepository<T>() where T : BaseEntity;
    }
}
