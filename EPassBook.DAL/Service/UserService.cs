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
        public IEnumerable<UserMaster> Get(Expression<Func<UserMaster, bool>> filter = null,
           Func<IQueryable<UserMaster>, IOrderedQueryable<UserMaster>> orderBy = null,
           string includeProperties = "")
        {
            IEnumerable<UserMaster> users = userMasterRepository.Get(filter,orderBy, includeProperties).ToList();
            return users;
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
        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }
        public UserMaster GetPassword(string userName)
        {
            return userMasterRepository.Get(w => w.UserName == userName && w.IsActive==true, null, string.Empty).FirstOrDefault();
        }        
    }
}
