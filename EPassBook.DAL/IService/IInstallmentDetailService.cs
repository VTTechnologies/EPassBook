using EPassBook.DAL.DBModel;
using System.Collections.Generic;

namespace EPassBook.DAL.IService
{
    public interface IInstallmentDetailService
    {
        IEnumerable<InstallmentDetail> GetAllInstallmentDetails();
        InstallmentDetail GetInstallmentDetailById(int id);
        void Insert(InstallmentDetail installmentDetail);
        void Update(InstallmentDetail installmentDetail);
        void Delete(int id);
        void SaveChanges();        
    }
}
