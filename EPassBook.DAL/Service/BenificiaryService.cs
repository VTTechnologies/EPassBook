using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.Service
{
    public class BenificiaryService : IBenificiaryService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<BenificiaryMaster> benificiaryMasterRepository;

        public BenificiaryService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            benificiaryMasterRepository = unitOfWork.GenericRepository<BenificiaryMaster>();
        }
        public IEnumerable<BenificiaryMaster> Get(Expression<Func<BenificiaryMaster, bool>> filter = null,
          Func<IQueryable<BenificiaryMaster>, IOrderedQueryable<BenificiaryMaster>> orderBy = null,
          string includeProperties = "")
        {
            IEnumerable<BenificiaryMaster> benificiaries = benificiaryMasterRepository.Get(filter, orderBy, includeProperties).ToList();
            return benificiaries;
        }

        public IEnumerable<BenificiaryMaster> GetAllBenificiaries()
        {
            IEnumerable<BenificiaryMaster> benificiaries = benificiaryMasterRepository.GetAll().ToList();
            return benificiaries;
        }

        public BenificiaryMaster GetBenificiaryById(int id)
        {
            BenificiaryMaster benficiary = benificiaryMasterRepository.GetById(id);
            return benficiary;
        }

        public void Add(BenificiaryMaster benificiary)
        {
            benificiaryMasterRepository.Add(benificiary);
        }       

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(BenificiaryMaster benificiary)
        {
            benificiaryMasterRepository.Update(benificiary);
        }

        public void Delete(int id)
        {
            benificiaryMasterRepository.Delete(id);
        }

        public int AuthenticateBeneficiary(long? userName, string password)
        {
            return benificiaryMasterRepository.GetAll().Where(w=>w.AdharNo==userName &&  w.Password==password).FirstOrDefault().BeneficiaryId;
        }
    }
}
