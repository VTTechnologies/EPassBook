using EPassBook.DAL.DBModel;
using EPassBook.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPassBook.DAL.Repository
{
   public  class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private EPassBookEntities dbContext = null;
        private DbSet<TEntity> entityTable = null;

        public GenericRepository()
        {
            this.dbContext = new EPassBookEntities();
            this.entityTable= dbContext.Set<TEntity>();
        }
        public IEnumerable<TEntity> GetAll()
        {
            return entityTable.ToList();
        }
        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = entityTable;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public TEntity GetById(int id)
        {
            return entityTable.Find(id);
        }
        public void Add(TEntity entity)
        {
            entityTable.Add(entity);
        }
        public void Update(TEntity entity)
        {
            entityTable.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            TEntity existing = entityTable.Find(id);
            entityTable.Remove(existing);
        }
        public virtual void Delete(TEntity entityToDelete)
        {
            if (dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                entityTable.Attach(entityToDelete);
            }
            entityTable.Remove(entityToDelete);
        }


           
               
    }
}
