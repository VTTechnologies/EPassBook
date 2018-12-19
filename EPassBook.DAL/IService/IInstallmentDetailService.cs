using EPassBook.DAL.DBModel;
using System.Collections.Generic;

namespace EPassBook.DAL.IService
{
    public interface IInstallmentDetailService
    {
        IEnumerable<InstallmentDetail> GetAllInstallmentDetails();
        InstallmentDetail GetInstallmentDetailById(int id);
        void Add(InstallmentDetail installmentDetail);
        void Update(InstallmentDetail installmentDetail);
        void Delete(int id);
        void SaveChanges();
        IEnumerable<sp_GetInstallmentListViewForUsersRoles_Result> GetInstallmentForLoginUsersWithStages(int StageID);

    }
}
