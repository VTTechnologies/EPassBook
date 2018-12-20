using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Web.Mvc;

namespace EPassBook.Controllers
{
    [ElmahError]
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        IUserService _userser;
        IBenificiary _Ibenificiary;
        IInstallmentDetailService _installmentDetailService;

        public HomeController(IUserService userser, IBenificiary Ibenificiary, IMapper mapper, IInstallmentDetailService installmentDetailService)
        {
            _userser = userser;
            _Ibenificiary = Ibenificiary;
            _mapper = mapper;
            _installmentDetailService = installmentDetailService;
        }

        [CustomAuthorize(Common.Admin)]
        public ActionResult Index()
        {
          
            return View();
        }

        //added by ather
        public ActionResult Website()
        {

            return View();
        }
        [CustomAuthorize(Common.Admin)]
        [HttpGet]
        public ActionResult AddUser()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddUser(UserViewModel userVM)
        {

            return View();
        }
        //end ather code

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Beneficiary()
        {
            return PartialView("Beneficiary", new BeneficiaryViewModel());
        }


        public ActionResult _InstallmentDetails()
        {
            var benfici = _Ibenificiary.GetBenificiaryById(1);            
            var benficiarymodel = _mapper.Map<BenificiaryMaster, BeneficiaryViewModel>(benfici);
            DateTime currentdate = Convert.ToDateTime(benficiarymodel.CreatedDate);            

            return PartialView(benficiarymodel);
        }

        [HttpGet]
        public ActionResult _CityHead()
        {
            InstallmentDetailsViewModel installvm = new InstallmentDetailsViewModel();
            var installment = _installmentDetailService.GetInstallmentDetailById(6);
            var installmentviewmodel = _mapper.Map<InstallmentDetail, InstallmentDetailsViewModel>(installment);
            installmentviewmodel._Comments = "";
            return PartialView(installmentviewmodel);
        }

        [HttpPost]
        public ActionResult _CityHead(InstallmentDetailsViewModel installmentDetailViewModel)
        {
            var installment = _installmentDetailService.GetInstallmentDetailById(installmentDetailViewModel.InstallmentId);

            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                installment.ModifiedBy = user.UserName;
                installment.CompanyID = user.CompanyID;

                installment.BeneficiaryAmnt = installmentDetailViewModel.BeneficiaryAmnt;
                installment.LoanAmnt = installmentDetailViewModel.LoanAmnt;
                installment.IsCentreAmnt = installmentDetailViewModel.IsCentreAmnt;
                installment.ConstructionLevel = installmentDetailViewModel.ConstructionLevel;
                installment.StageID = (int)Common.WorkFlowStages.ProjectManager;
                installment.InstallmentNo = installmentDetailViewModel.InstallmentNo;
                installment.ModifiedDate = DateTime.Now;

                var comments = new Comment();
                comments.Comments = installmentDetailViewModel._Comments;
                comments.CreatedBy = user.UserName;
                comments.BeneficiaryId = installmentDetailViewModel.BeneficiaryId;
                comments.CreatedDate = DateTime.Now;
                comments.CompanyID = user.CompanyID;

                installment.Comments.Add(comments);

                _installmentDetailService.Update(installment);
                _installmentDetailService.SaveChanges();
            }
            return View();
        }

    }
}