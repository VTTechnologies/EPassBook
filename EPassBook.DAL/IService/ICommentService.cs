using EPassBook.DAL.DBModel;
using EPassBook.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace EPassBook.DAL.IService
{
    public interface ICommentService
    {
        IEnumerable<Comment> Get(Expression<Func<Comment, bool>> filter = null,
          Func<IQueryable<Comment>, IOrderedQueryable<Comment>> orderBy = null,
          string includeProperties = "");
        IEnumerable<Comment> GetAllComments();
        Comment GetCommentById(int id);
        IEnumerable<sp_GetSurveyDetailsByBenID_Result> GetSurveyDetailsByBenificiaryID(int id,int installmentNo);
        void Add(Comment user);
        void Update(Comment user);
        void Delete(int id);
        void SaveChanges();
    }
}
