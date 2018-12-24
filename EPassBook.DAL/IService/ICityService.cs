using EPassBook.DAL.DBModel;
using System.Collections.Generic;

namespace EPassBook.DAL.IService
{
    public interface ICityService
    {
        IEnumerable<CityMaster> GetAllCities();
        CityMaster GetCityById(int id);
        void Add(CityMaster city);
        void Update(CityMaster city);
        void Delete(int id);
        void SaveChanges();        
    }
}
