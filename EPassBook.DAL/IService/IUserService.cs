using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EPassBook.DAL.IService
{
    public interface IUserService
    {
        IEnumerable<UserMaster> Get(Expression<Func<UserMaster, bool>> filter = null,
           Func<IQueryable<UserMaster>, IOrderedQueryable<UserMaster>> orderBy = null,
           string includeProperties = "");
        IEnumerable<UserMaster> GetAllUsers();
        UserMaster GetUserById(int id);
        void Add(UserMaster user);
        void Update(UserMaster user);
        void Delete(int id);
        void SaveChanges();
        UserMaster GetPassword(string userName);
    }
}
