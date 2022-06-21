using System.Linq.Expressions;

namespace IMS_LEARN.Services.Base
{
    public interface IBaseRepository<T> where T : class
    {
        IQueryable<T> GetAllQueryable(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = null);

        T SingleOrDefault(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

        T FirstOrDefault(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);

        T Insert(T entity);
        void InsertRange(IEnumerable<T> entities);

        T Update(T entity);
        public void UpdateRange(IEnumerable<T> entities);

        T Delete(T entiry);
        public void DeleteRange(IEnumerable<T> entities);

        T GetById(object id);

        void Save();
    }
}
