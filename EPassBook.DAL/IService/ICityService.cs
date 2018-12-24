using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.IService
{
    public interface ICityService
    {
        IEnumerable<CityMaster> Get(Expression<Func<CityMaster, bool>> filter = null,
        Func<IQueryable<CityMaster>, IOrderedQueryable<CityMaster>> orderBy = null,
        string includeProperties = "");
        IEnumerable<CityMaster> GetAllCities();
        CityMaster GetCityById(int id);
        void Add(CityMaster city);
        void Update(CityMaster city);
        void Delete(int id);
        void SaveChanges();        
    }
}
