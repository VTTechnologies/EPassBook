using EPassBook.DAL.DBModel;
using EPassBook.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPassBook.DAL.IService
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetAllComments();
        Comment GetCommentById(int id);
        IEnumerable<sp_GetSurveyDetailsByBenID_Result> GetSurveyDetailsByBenificiaryID(int id);
        void Add(Comment user);
        void Update(Comment user);
        void Delete(int id);
        void SaveChanges();
    }
}
