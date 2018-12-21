using EPassBook.DAL.DBModel;
using System.Collections.Generic;

namespace EPassBook.DAL.IService
{
    public interface ICompanyMasterService
    {
        IEnumerable<CompanyMaster> GetAllCompanies();
        CompanyMaster GetCompanyById(int id);
        void Add(CompanyMaster city);
        void Update(CompanyMaster city);
        void Delete(int id);
        void SaveChanges();
    }
}