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
    public class UserService : IUserService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<UserMaster> userMasterRepository;

        public UserService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            userMasterRepository = unitOfWork.GenericRepository<UserMaster>();
        }

        public IEnumerable<UserMaster> GetAllUsers()
        {
            IEnumerable<UserMaster> users = userMasterRepository.GetAll().ToList();
            return users;
        }
        public UserMaster GetUserById(int id)
        {
           UserMaster user = userMasterRepository.GetById(id);
            return user;
        }

        public void Add(UserMaster user)
        {
            userMasterRepository.Add(user);
        }
        public void Update(UserMaster user)
        {
            userMasterRepository.Update(user);
        }
        public void Delete(int id)
        {
            userMasterRepository.Delete(id);
        }
    }
}
