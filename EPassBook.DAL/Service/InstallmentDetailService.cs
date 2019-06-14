using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.Service
{
    public class InstallmentDetailService : IInstallmentDetailService
    {
        private readonly epassbook_dbEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<InstallmentDetail> installmentDetailRepository;

        public InstallmentDetailService()
        {
            _dbContext = new epassbook_dbEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            installmentDetailRepository = unitOfWork.GenericRepository<InstallmentDetail>();
        }

        public IEnumerable<InstallmentDetail> Get(Expression<Func<InstallmentDetail, bool>> filter = null,
        Func<IQueryable<InstallmentDetail>, IOrderedQueryable<InstallmentDetail>> orderBy = null,
        string includeProperties = "")
        {
            IEnumerable<InstallmentDetail> installmentDetails = installmentDetailRepository.Get(filter, orderBy, includeProperties).ToList();
            return installmentDetails;
        }


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InstallmentDetail> GetAllInstallmentDetails()
        {
            IEnumerable<InstallmentDetail> benficimaster = installmentDetailRepository.GetAll().ToList();
            return benficimaster;
        }

        public InstallmentDetail GetInstallmentDetailById(int id)
        {
            InstallmentDetail benficiaries = installmentDetailRepository.GetById(id);
            return benficiaries;
        }

        public void Add(InstallmentDetail installmentDetail)
        {
            installmentDetailRepository.Add(installmentDetail);            
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(InstallmentDetail installmentDetail)
        {
            installmentDetailRepository.Update(installmentDetail);
        }

        IEnumerable<sp_GetInstallmentListViewForUsersRoles_Result> IInstallmentDetailService.GetInstallmentForLoginUsersWithStages(string StageID)
        {
            var InstallmentDetailsViewList = _dbContext.sp_GetInstallmentListViewForUsersRoles(StageID);
            //parameter added for testing only
            return InstallmentDetailsViewList.ToList();
        }
    }
}
