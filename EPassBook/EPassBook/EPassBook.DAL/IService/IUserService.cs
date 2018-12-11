using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPassBook.DAL.IService
{
    public interface IUserService
    {
        IEnumerable<UserMaster> GetAllUsers();
        UserMaster GetUserById(int id);
        void Add(UserMaster user);
        void Update(UserMaster user);
        void Delete(int id);
    }
}
