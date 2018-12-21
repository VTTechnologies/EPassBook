using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPassBook.DAL.Service
{
     public class CompanyMasterService : ICompanyMasterService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<CompanyMaster> CompanyMasterRepository;

        public CompanyMasterService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            CompanyMasterRepository = unitOfWork.GenericRepository<CompanyMaster>();
        }

        public void Delete(int id)
        {
            CompanyMasterRepository.Delete(id);
        }

        public IEnumerable<CompanyMaster> GetAllCompanies()
        {
            IEnumerable<CompanyMaster> Allcompany = CompanyMasterRepository.GetAll().ToList();
            return Allcompany;
        }

        public CompanyMaster GetCompanyById(int id)
        {
            CompanyMaster company = CompanyMasterRepository.GetById(id);
            return company;
        }

        public void Add(CompanyMaster company)
        {
            CompanyMasterRepository.Add(company);
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(CompanyMaster company)
        {
            CompanyMasterRepository.Update(company);
        }
    }
}
