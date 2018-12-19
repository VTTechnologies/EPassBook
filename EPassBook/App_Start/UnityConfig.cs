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
                cfg.CreateMap<UserMaster, UserViewModel>();
                cfg.CreateMap<BenificiaryMaster, BeneficiaryViewModel>();
                cfg.CreateMap<InstallmentDetail, InstallmentDetailsViewModel>();
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

            //Created instance of mapper into unity
            container.RegisterInstance(mapper);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}