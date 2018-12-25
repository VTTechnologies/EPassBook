using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.IService
{
    public interface IInstallmentDetailService
    {
         IEnumerable<InstallmentDetail> Get(Expression<Func<InstallmentDetail, bool>> filter = null,
        Func<IQueryable<InstallmentDetail>, IOrderedQueryable<InstallmentDetail>> orderBy = null,
        string includeProperties = "");
        IEnumerable<InstallmentDetail> GetAllInstallmentDetails();
        InstallmentDetail GetInstallmentDetailById(int id);
        void Add(InstallmentDetail installmentDetail);
        void Update(InstallmentDetail installmentDetail);
        void Delete(int id);
        void SaveChanges();
        IEnumerable<sp_GetInstallmentListViewForUsersRoles_Result> GetInstallmentForLoginUsersWithStages(int StageID);

    }
}
