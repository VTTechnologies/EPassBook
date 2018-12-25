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
    public class UserInRoleService : IUserInRoleService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<UserInRole> UserInRoleRepository;

        public UserInRoleService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            UserInRoleRepository = unitOfWork.GenericRepository<UserInRole>();
        }
        public IEnumerable<UserInRole> Get(Expression<Func<UserInRole, bool>> filter = null,
           Func<IQueryable<UserInRole>, IOrderedQueryable<UserInRole>> orderBy = null,
           string includeProperties = "")
        {
            IEnumerable<UserInRole> UserInRoles = UserInRoleRepository.Get(filter,orderBy, includeProperties).ToList();
            return UserInRoles;
        }
        public IEnumerable<UserInRole> GetAllUserInRoles()
        {
            IEnumerable<UserInRole> UserInRoles = UserInRoleRepository.GetAll().ToList();
            return UserInRoles;
        }
        public UserInRole GetUserInRoleById(int id)
        {
            UserInRole UserInRole = UserInRoleRepository.GetById(id);
            return UserInRole;
        }

        public void Add(UserInRole UserInRole)
        {
            UserInRoleRepository.Add(UserInRole);
        }
        public void Update(UserInRole UserInRole)
        {
            UserInRoleRepository.Update(UserInRole);
        }
        public void Delete(int id)
        {
            UserInRoleRepository.Delete(id);
        }
        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }
    }
}
