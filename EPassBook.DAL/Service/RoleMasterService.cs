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
     public class RoleMasterService : IRoleMasterService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<RoleMaster> RoleMasterRepository;

        public RoleMasterService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            RoleMasterRepository = unitOfWork.GenericRepository<RoleMaster>();
        }

        public void Delete(int id)
        {
            RoleMasterRepository.Delete(id);
        }

        public IEnumerable<RoleMaster> GetAllRoles()
        {
            IEnumerable<RoleMaster> Allroles = RoleMasterRepository.GetAll().ToList();
            return Allroles;
        }

        public RoleMaster GetRoleById(int id)
        {
            RoleMaster role = RoleMasterRepository.GetById(id);
            return role;
        }

        public void Add(RoleMaster role)
        {
            RoleMasterRepository.Add(role);
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(RoleMaster role)
        {
            RoleMasterRepository.Update(role);
        }
        //public IEnumerable<RoleMaster> GetAllActiveRoles()
        //{
        //    List<RoleMaster> AllActiveRoles = RoleMasterRepository.Get(r => r.IsActive == true).Select(r => r.IsActive == true).ToList();
            
        //}
    }
}
