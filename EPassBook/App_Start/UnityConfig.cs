using AutoMapper;
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
            //AutoMapper Configuration starts here
            var config = new MapperConfiguration(cfg =>
            {
                //Create all maps here



                //            Mapper.Map<OrderLine, OrderLineDTO>()
                //.ForMember(m => m.Order, opt => opt.Ignore());

                //            Mapper.Map<Order, OrderDTO>()
                //                .AfterMap((src, dest) =>
                //                {
                //                    foreach (var i in dest.OrderLines)
                //                        i.Order = dest;
                //                });

                //cfg.CreateMap<UserMaster, UserViewModel>().MaxDepth(5);
                cfg.CreateMap<BenificiaryMaster, BeneficiaryViewModel>();
                cfg.CreateMap<InstallmentDetail, InstallmentDetailsViewModel>();
                cfg.CreateMap<InstallmentDetail, AccountDetailsViewModel>();
                cfg.CreateMap<BenificiaryMaster, AccountDetailsViewModel>();
                cfg.CreateMap<WorkflowStage, WorkFlowStagesViewModel>();
                cfg.CreateMap<sp_GetInstallmentListViewForUsersRoles_Result, InstallmentListView>();
                //cfg.CreateMap<UserViewModel, UserMaster>();
                cfg.CreateMap<UserInRoleViewModel, UserInRole>();

                cfg.CreateMap<UserMaster, UserViewModel>()
    .ForSourceMember(source => source.CompanyMaster, opt => opt.Ignore());
                cfg.CreateMap<UserViewModel, UserMaster > ()
    .ForSourceMember(source => source.CompanyMaster, opt => opt.Ignore());
                //cfg.CreateMap<MyHappyEntity, MyHappyEntityDto>();

                //...
            });
            IMapper mapper = config.CreateMapper();

            //Unity Configuration starts here
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IBenificiary, BenificiaryService>();
            container.RegisterType<ICommentService, CommentService>();
            container.RegisterType<IInstallmentDetailService, InstallmentDetailService>();
            container.RegisterType<IWorkFlowStagesService, WorkFlowStagesService>();
            container.RegisterType<ICityService, CityMasterService>();
            container.RegisterType<IRoleMasterService, RoleMasterService>();
            container.RegisterType<ICompanyMasterService, CompanyMasterService>();

            //Created instance of mapper into unity
            container.RegisterInstance(mapper);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}