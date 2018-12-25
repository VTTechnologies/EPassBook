using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.IService
{
    public interface IUserInRoleService
    {
         IEnumerable<UserInRole> Get(Expression<Func<UserInRole, bool>> filter = null,
       Func<IQueryable<UserInRole>, IOrderedQueryable<UserInRole>> orderBy = null,
       string includeProperties = "");
        IEnumerable<UserInRole> GetAllUserInRoles();
        UserInRole GetUserInRoleById(int id);
        void Add(UserInRole userInRoles);
        void Update(UserInRole userInRoles);
        void Delete(int id);
        void SaveChanges();        
    }
}
