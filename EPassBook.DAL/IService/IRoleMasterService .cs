using EPassBook.DAL.DBModel;
using System.Collections.Generic;

namespace EPassBook.DAL.IService
{
    public interface IRoleMasterService
    {
        IEnumerable<RoleMaster> GetAllRoles();
        RoleMaster GetRoleById(int id);
        void Add(RoleMaster city);
        void Update(RoleMaster city);
        void Delete(int id);
        void SaveChanges();        
    }
}
