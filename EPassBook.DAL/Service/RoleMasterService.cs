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
     public class RoleMasterService : IRoleMasterService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<RoleMaster> roleMasterRepository;

        public RoleMasterService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            roleMasterRepository = unitOfWork.GenericRepository<RoleMaster>();
        }

        public IEnumerable<RoleMaster> Get(Expression<Func<RoleMaster, bool>> filter = null,
       Func<IQueryable<RoleMaster>, IOrderedQueryable<RoleMaster>> orderBy = null,
       string includeProperties = "")
        {
            IEnumerable<RoleMaster> roles = roleMasterRepository.Get(filter, orderBy, includeProperties).ToList();
            return roles;
        }

        public void Delete(int id)
        {
            roleMasterRepository.Delete(id);
        }

        public IEnumerable<RoleMaster> GetAllRoles()
        {
            IEnumerable<RoleMaster> Allroles = roleMasterRepository.GetAll().ToList();
            return Allroles;
        }

        public RoleMaster GetRoleById(int id)
        {
            RoleMaster role = roleMasterRepository.GetById(id);
            return role;
        }

        public void Add(RoleMaster role)
        {
            roleMasterRepository.Add(role);
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(RoleMaster role)
        {
            roleMasterRepository.Update(role);
        }
        //public IEnumerable<RoleMaster> GetAllActiveRoles()
        //{
        //    List<RoleMaster> AllActiveRoles = RoleMasterRepository.Get(r => r.IsActive == true).Select(r => r.IsActive == true).ToList();
            
        //}
    }
}
