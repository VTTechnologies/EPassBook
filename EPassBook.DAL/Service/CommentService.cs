using EPassBook.DAL.DBModel;
using EPassBook.DAL;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPassBook.DAL.Service
{
    public class CommentService : ICommentService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<Comment> commentRepository;

        public CommentService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            commentRepository = unitOfWork.GenericRepository<Comment>();
        }
        public IEnumerable<Comment> GetAllComments()
        {
            IEnumerable<Comment> comment = commentRepository.GetAll().ToList();
            return comment;
        }
        public Comment GetCommentById(int id)
        {
            Comment comment = commentRepository.GetById(id);
            return comment;
        }

        public void Add(Comment comment)
        {
            commentRepository.Add(comment);
        }
        public void Update(Comment comment)
        {
            commentRepository.Update(comment);
        }
        public void Delete(int id)
        {
            commentRepository.Delete(id);
        }

        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        IEnumerable<sp_GetSurveyDetailsByBenID_Result> ICommentService.GetSurveyDetailsByBenificiaryID(int id)
        {
            var surveyDetails = _dbContext.sp_GetSurveyDetailsByBenID(id);    
            //parameter added for testing only
            return surveyDetails.ToList();
        }
    }
}
