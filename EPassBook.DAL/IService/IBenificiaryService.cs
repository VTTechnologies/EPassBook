using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.IService
{
    public interface IBenificiaryService
    {
        IEnumerable<BenificiaryMaster> Get(Expression<Func<BenificiaryMaster, bool>> filter = null,
         Func<IQueryable<BenificiaryMaster>, IOrderedQueryable<BenificiaryMaster>> orderBy = null,
         string includeProperties = "");
        IEnumerable<BenificiaryMaster> GetAllBenificiaries();
        BenificiaryMaster GetBenificiaryById(int id);
        void Add(BenificiaryMaster benificiary);
        void Update(BenificiaryMaster benificiary);
        BenificiaryMaster AuthenticateBeneficiary(long? userName, string password);
        void Delete(int id);
        void SaveChanges();        
    }
}
