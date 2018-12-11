using EPassBook.DAL.DBModel;
using EPassBook.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPassBook.DAL.Repository
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly EPassBookEntities _dbContext;
        private Dictionary<string, object> repositories;
        public UnitOfWork(EPassBookEntities context)

        {
            _dbContext = context;
        }
        private bool _disposed;

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, 
        ///     releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        ///     Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposed)
                return;

            _disposed = true;
        }
        public void SaveChanges()
        {
            ((IObjectContextAdapter)_dbContext).ObjectContext.SaveChanges();
        }

        public GenericRepository<TEntity> GenericRepository<TEntity>() where TEntity: class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var type = typeof(TEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);
                repositories.Add(type, repositoryInstance);
            }
            return (GenericRepository<TEntity>)repositories[type];
        }
    }
}
