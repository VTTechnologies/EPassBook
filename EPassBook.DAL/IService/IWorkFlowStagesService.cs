using EPassBook.DAL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.IService
{
    public interface IWorkFlowStagesService
    {
        IEnumerable<WorkflowStage> Get(Expression<Func<WorkflowStage, bool>> filter = null,
       Func<IQueryable<WorkflowStage>, IOrderedQueryable<WorkflowStage>> orderBy = null,
       string includeProperties = "");
        IEnumerable<WorkflowStage> GetAllWorkflowStages();
        //WorkflowStage GetWorkflowStageById(int id);
        void Add(WorkflowStage WorkflowStage);
        void Update(WorkflowStage WorkflowStage);
        void Delete(int id);
        void SaveChanges();
        List<int?> GetWorkflowStageById(List<int> roleIds);
    }
}
