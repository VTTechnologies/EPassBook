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
     public class CityMasterService: ICityService
    {
         private readonly epassbook_dbEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<CityMaster> cityMasterRepository;

        public CityMasterService()
        {
            _dbContext = new epassbook_dbEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            cityMasterRepository = unitOfWork.GenericRepository<CityMaster>();
        }
        public IEnumerable<CityMaster> Get(Expression<Func<CityMaster, bool>> filter = null,
        Func<IQueryable<CityMaster>, IOrderedQueryable<CityMaster>> orderBy = null,
        string includeProperties = "")
        {
            IEnumerable<CityMaster> cities = cityMasterRepository.Get(filter, orderBy, includeProperties).ToList();
            return cities;
        }

        public void Delete(int id)
        {
            cityMasterRepository.Delete(id);
        }

        public IEnumerable<CityMaster> GetAllCities()
        {
            IEnumerable<CityMaster> Allcities = cityMasterRepository.GetAll().ToList();
            return Allcities;
        }

        public CityMaster GetCityById(int id)
        {
            CityMaster city = cityMasterRepository.GetById(id);
            return city;
        }

        public void Add(CityMaster city)
        {
            cityMasterRepository.Add(city);
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(CityMaster city)
        {
            cityMasterRepository.Update(city);
        }
    }
}
