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

        public void Add(InstallmentDetail installmentDetail)
        {
            InstallmentDetailRepository.Add(installmentDetail);            
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(InstallmentDetail installmentDetail)
        {
            InstallmentDetailRepository.Update(installmentDetail);
        }

        IEnumerable<sp_GetInstallmentListViewForUsersRoles_Result> IInstallmentDetailService.GetInstallmentForLoginUsersWithStages(int StageID)
        {
            var InstallmentDetailsViewList = _dbContext.sp_GetInstallmentListViewForUsersRoles(StageID);
            //parameter added for testing only
            return InstallmentDetailsViewList.ToList();
        }
    }
}
