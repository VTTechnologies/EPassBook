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
        private readonly EPassBookEntities _dbContext;
        private UnitOfWork unitOfWork;
        private GenericRepository<WorkflowStage> workflowStageRepository;

        public WorkFlowStagesService()
        {
            _dbContext = new EPassBookEntities();
            unitOfWork = new UnitOfWork(_dbContext);
            workflowStageRepository = unitOfWork.GenericRepository<WorkflowStage>();
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

        public List<int> GetWorkflowStageById(List<int> roleIds)
        {
            List<StageInRole> lststageInRoles = new List<StageInRole>();
            StageInRole stageInRole = new StageInRole();
            foreach (var item in roleIds)
            {
                stageInRole.RoleId = item;
                lststageInRoles.Add(stageInRole);
            }
            var a = workflowStageRepository.GetAll().Where(w => w.StageInRoles.Intersect(lststageInRoles).Any()).Select(s=>s.StageId);
            return a.ToList(); 
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
