using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPassBook.DAL.Service
{
     public class CompanyMasterService : ICompanyMasterService
    {
         private readonly epassbook_dbEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<CompanyMaster> companyMasterRepository;

        public CompanyMasterService()
        {
            _dbContext = new epassbook_dbEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            companyMasterRepository = unitOfWork.GenericRepository<CompanyMaster>();
        }

        public IEnumerable<CompanyMaster> Get(Expression<Func<CompanyMaster, bool>> filter = null,
         Func<IQueryable<CompanyMaster>, IOrderedQueryable<CompanyMaster>> orderBy = null,
         string includeProperties = "")
        {
            IEnumerable<CompanyMaster> compamies = companyMasterRepository.Get(filter, orderBy, includeProperties).ToList();
            return compamies;
        }

        public void Delete(int id)
        {
            companyMasterRepository.Delete(id);
        }

        public IEnumerable<CompanyMaster> GetAllCompanies()
        {
            IEnumerable<CompanyMaster> compamies = companyMasterRepository.GetAll().ToList();
            return compamies;
        }

        public CompanyMaster GetCompanyById(int id)
        {
            CompanyMaster company = companyMasterRepository.GetById(id);
            return company;
        }

        public void Add(CompanyMaster company)
        {
            companyMasterRepository.Add(company);
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(CompanyMaster company)
        {
            companyMasterRepository.Update(company);
        }
    }
}
