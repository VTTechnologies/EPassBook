using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPassBook.DAL.Service
{
    public class WorkFlowStagesService : IWorkFlowStagesService
    {
        private readonly epassbook_dbEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<WorkflowStage> workflowStageRepository;
        private GenericRepository<StageInRole> stageInRoleRepository;
        public WorkFlowStagesService()
        {
            _dbContext = new epassbook_dbEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            workflowStageRepository = unitOfWork.GenericRepository<WorkflowStage>();
            stageInRoleRepository= unitOfWork.GenericRepository<StageInRole>();
        }

        public IEnumerable<WorkflowStage> Get(Expression<Func<WorkflowStage, bool>> filter = null,
       Func<IQueryable<WorkflowStage>, IOrderedQueryable<WorkflowStage>> orderBy = null,
       string includeProperties = "")
        {
            IEnumerable<WorkflowStage> workflowStages = workflowStageRepository.Get(filter, orderBy, includeProperties).ToList();
            return workflowStages;
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

        public List<int?> GetWorkflowStageById(List<int> roleIds)
        {
            List<StageInRole> lststageInRoles = new List<StageInRole>();
            List<int?> stages = new List<int?>();
            foreach (var role in roleIds)
            {
                var stage = stageInRoleRepository.Get(w=>w.RoleId==role).Select(s => s.StageId).ToList();
                stages.AddRange(stage);               
            }
            return stages; 
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
