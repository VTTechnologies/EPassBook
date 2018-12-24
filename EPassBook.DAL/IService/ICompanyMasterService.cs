using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.IService
{
    public interface ICompanyMasterService
    {
         IEnumerable<CompanyMaster> Get(Expression<Func<CompanyMaster, bool>> filter = null,
         Func<IQueryable<CompanyMaster>, IOrderedQueryable<CompanyMaster>> orderBy = null,
         string includeProperties = "");
        IEnumerable<CompanyMaster> GetAllCompanies();
        CompanyMaster GetCompanyById(int id);
        void Add(CompanyMaster city);
        void Update(CompanyMaster city);
        void Delete(int id);
        void SaveChanges();
    }
}