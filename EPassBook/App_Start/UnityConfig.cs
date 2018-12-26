using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.DAL.Service;
using EPassBook.Models;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace EPassBook
{
    public static class UnityConfig
    {
        

        public static void RegisterComponents()
        {
            
            //Unity Configuration starts here
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IBenificiaryService, BenificiaryService>();
            container.RegisterType<ICommentService, CommentService>();
            container.RegisterType<IInstallmentDetailService, InstallmentDetailService>();
            container.RegisterType<IWorkFlowStagesService, WorkFlowStagesService>();
            container.RegisterType<ICityService, CityMasterService>();
            container.RegisterType<IRoleMasterService, RoleMasterService>();
            container.RegisterType<ICompanyMasterService, CompanyMasterService>();
            container.RegisterType<IUserInRoleService, UserInRoleService>();

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}