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
     public class CityMasterService: ICityService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<CityMaster> CityMasterRepository;

        public CityMasterService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            CityMasterRepository = unitOfWork.GenericRepository<CityMaster>();
        }

        public void Delete(int id)
        {
            CityMasterRepository.Delete(id);
        }

        public IEnumerable<CityMaster> GetAllCities()
        {
            IEnumerable<CityMaster> Allcities = CityMasterRepository.GetAll().ToList();
            return Allcities;
        }

        public CityMaster GetCityById(int id)
        {
            CityMaster city = CityMasterRepository.GetById(id);
            return city;
        }

        public void Add(CityMaster city)
        {
            CityMasterRepository.Add(city);
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(CityMaster city)
        {
            CityMasterRepository.Update(city);
        }
    }
}
