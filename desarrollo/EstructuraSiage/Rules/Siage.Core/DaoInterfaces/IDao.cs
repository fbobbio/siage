using System.Collections.Generic;

namespace Siage.Core.DaoInterfaces
{
    public interface IDao<TEntity, TId>
    {
        TEntity GetById(TId id, bool shouldLock);
        TEntity GetById(TId id);
        List<TEntity> GetAll();
        List<TEntity> GetAll(string orderby);
        TEntity Save(TEntity entity);
        TEntity SaveOrUpdate(TEntity entity);
        void Delete(TEntity entity);
        TEntity MakePersistent(TEntity entity);
    }
}
