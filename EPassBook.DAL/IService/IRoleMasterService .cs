using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.IService
{
    public interface IRoleMasterService
    {
         IEnumerable<RoleMaster> Get(Expression<Func<RoleMaster, bool>> filter = null,
       Func<IQueryable<RoleMaster>, IOrderedQueryable<RoleMaster>> orderBy = null,
       string includeProperties = "");
        IEnumerable<RoleMaster> GetAllRoles();
        RoleMaster GetRoleById(int id);
        void Add(RoleMaster city);
        void Update(RoleMaster city);
        void Delete(int id);
        void SaveChanges();        
    }
}
