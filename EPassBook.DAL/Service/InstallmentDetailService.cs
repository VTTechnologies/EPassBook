using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPassBook.DAL.Service
{
    public class InstallmentDetailService : IInstallmentDetailService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<InstallmentDetail> InstallmentDetailRepository;

        public InstallmentDetailService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            InstallmentDetailRepository = unitOfWork.GenericRepository<InstallmentDetail>();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InstallmentDetail> GetAllInstallmentDetails()
        {
            IEnumerable<InstallmentDetail> benficimaster = InstallmentDetailRepository.GetAll().ToList();
            return benficimaster;
        }

        public InstallmentDetail GetInstallmentDetailById(int id)
        {
            InstallmentDetail benficiaries = InstallmentDetailRepository.GetById(id);
            return benficiaries;
        }

        public void Insert(InstallmentDetail installmentDetail)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(InstallmentDetail installmentDetail)
        {
            throw new NotImplementedException();
        }
    }
}
