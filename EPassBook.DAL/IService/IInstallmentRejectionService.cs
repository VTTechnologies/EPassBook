using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.IService
{
    public interface IInstallmentRejectionService
    {
         IEnumerable<InstallmentRejection> Get(Expression<Func<InstallmentRejection, bool>> filter = null,
        Func<IQueryable<InstallmentRejection>, IOrderedQueryable<InstallmentRejection>> orderBy = null,
        string includeProperties = "");
        IEnumerable<InstallmentRejection> GetAllInstallmentRejections();
        InstallmentRejection GetInstallmentRejectionById(int id);
        void Add(InstallmentRejection installmentRejection);
        void Update(InstallmentRejection installmentRejection);
        void Delete(int id);
        void SaveChanges();
    }
}
