using EPassBook.DAL.DBModel;
using System.Collections.Generic;

namespace EPassBook.DAL.IService
{
    public interface IBenificiary
    {
        IEnumerable<BenificiaryMaster> GetAllBenificiaries();
        BenificiaryMaster GetBenificiaryById(int id);
        void Add(BenificiaryMaster benificiary);
        void Update(BenificiaryMaster benificiary);
        void Delete(int id);
        void SaveChanges();        
    }
}
