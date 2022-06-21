using IMS_LEARN.Infratructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IMS_LEARN.Services.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly ImsDbContext _context;
        public DbSet<T> dbSet;

        public BaseRepository(ImsDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query;
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.FirstOrDefault();
        }

        public T Insert(T entity)
        {
            T thisEntity = _context.Set<T>().Add(entity).Entity;
            if (_context.SaveChanges() > 0)
            {
                return thisEntity;
            }

            return null;
        }

        public void InsertRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }


        public T Delete(T entiry)
        {
            try
            {
                _context.Set<T>().Remove(entiry);
                _context.SaveChanges();
                return entiry;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        

       

        public T GetById(object id)
        {
            throw new NotImplementedException();
        }

        

        public void Save()
        {
            throw new NotImplementedException();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            if (_context.SaveChanges() > 0)
            {
                return entity;
            }
            return null;
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }
    }
}
