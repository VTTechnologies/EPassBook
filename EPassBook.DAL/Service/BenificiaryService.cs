using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPassBook.DAL.Service
{
    public class BenificiaryService : IBenificiary
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<BenificiaryMaster> BenificiaryMasterRepository;

        public BenificiaryService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            BenificiaryMasterRepository = unitOfWork.GenericRepository<BenificiaryMaster>();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BenificiaryMaster> GetAllBenificiaries()
        {
            IEnumerable<BenificiaryMaster> benficimaster = BenificiaryMasterRepository.GetAll().ToList();
            return benficimaster;
        }

        public BenificiaryMaster GetBenificiaryById(int id)
        {
            BenificiaryMaster benficiaries = BenificiaryMasterRepository.GetById(id);
            return benficiaries;
        }

        public void Add(BenificiaryMaster benificiary)
        {
            BenificiaryMasterRepository.Add(benificiary);
        }
        //public void Add(InstallmentDetail installmentDetail)
        //{
        //    InstallmentDetailRepository.Add(installmentDetail);
        //}

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(BenificiaryMaster benificiary)
        {
            throw new NotImplementedException();
        }
    }
}
