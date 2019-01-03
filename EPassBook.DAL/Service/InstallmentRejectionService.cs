using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.Service
{
    public class InstallmentRejectionService : IInstallmentRejectionService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<InstallmentRejection> installmentRejectionRepository;

        public InstallmentRejectionService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            installmentRejectionRepository = unitOfWork.GenericRepository<InstallmentRejection>();
        }

        public IEnumerable<InstallmentRejection> Get(Expression<Func<InstallmentRejection, bool>> filter = null,
        Func<IQueryable<InstallmentRejection>, IOrderedQueryable<InstallmentRejection>> orderBy = null,
        string includeProperties = "")
        {
            IEnumerable<InstallmentRejection> installmentRejection = installmentRejectionRepository.Get(filter, orderBy, includeProperties).ToList();
            return installmentRejection;
        }


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InstallmentRejection> GetAllInstallmentRejections()
        {
            IEnumerable<InstallmentRejection> installmentRejection = installmentRejectionRepository.GetAll().ToList();
            return installmentRejection;
        }

        public InstallmentRejection GetInstallmentRejectionById(int id)
        {
            InstallmentRejection benficiaries = installmentRejectionRepository.GetById(id);
            return benficiaries;
        }

        public void Add(InstallmentRejection InstallmentRejection)
        {
            installmentRejectionRepository.Add(InstallmentRejection);            
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(InstallmentRejection InstallmentRejection)
        {
            installmentRejectionRepository.Update(InstallmentRejection);
        }
    }
}
