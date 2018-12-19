using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPassBook.DAL.Service
{
    public class WorkFlowStagesService : IWorkFlowStagesService
    {
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<WorkflowStage> workflowStageRepository;

        public WorkFlowStagesService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            workflowStageRepository = unitOfWork.GenericRepository<WorkflowStage>();
        }

        public void Add(WorkflowStage WorkflowStage)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WorkflowStage> GetAllWorkflowStages()
        {
            return workflowStageRepository.Get();
        }

        public int GetUserStageByRoleID(int? roleId)
        {
          return  workflowStageRepository.Get().Where(w=>w.StageInRoles.Where(we=>we.RoleId== roleId).Select(s=>s.StageId).Any()).FirstOrDefault().StageId;
        }

        public WorkflowStage GetWorkflowStageById(int id)
        {
            return workflowStageRepository.GetById(id);
        }

       
        public void SaveChanges()
        {
            unitOfWork.SaveChanges();
        }

        public void Update(WorkflowStage WorkflowStage)
        {
            throw new NotImplementedException();
        }
    }
}
