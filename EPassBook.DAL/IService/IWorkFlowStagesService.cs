using EPassBook.DAL.DBModel;
using System.Collections.Generic;

namespace EPassBook.DAL.IService
{
    public interface IWorkFlowStagesService
    {
        IEnumerable<WorkflowStage> GetAllWorkflowStages();
        WorkflowStage GetWorkflowStageById(int id);
        void Add(WorkflowStage WorkflowStage);
        void Update(WorkflowStage WorkflowStage);
        void Delete(int id);
        void SaveChanges();
        int GetUserStageByRoleID(int? roleId);
    }
}
