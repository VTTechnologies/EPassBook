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
            return PartialView();
        }

        [HttpPost]
        public ActionResult _CityHead(InstallmentDetailsViewModel installmentDetailViewModel)
        {
            var installment = new InstallmentDetail();

            //if (ModelState.IsValid)
            //{
                installment.BeneficiaryId = 1;//installmentDetailViewModel.BeneficiaryId;
                installment.BeneficiaryAmnt = installmentDetailViewModel.BeneficiaryAmnt;
                installment.LoanAmnt = installmentDetailViewModel.LoanAmnt;
                installment.IsCentreAmnt = installmentDetailViewModel.IsCentreAmnt;
                installment.ConstructionLevel = installmentDetailViewModel.ConstructionLevel;
                installment.StageID = 1;
                installment.InstallmentNo = 1;
                installment.CreatedDate = DateTime.Now;
                installment.CreatedBy = "Admin";
                installment.CompanyID = 1;

                var comments = new Comment();
                comments.Comments = installmentDetailViewModel._Comments;
                comments.CreatedBy = "Admin";
            comments.BeneficiaryId = 1;
                comments.CreatedDate = DateTime.Now;
                comments.CompanyID = 1;

                installment.Comments.Add(comments);

                _installmentDetailService.Insert(installment);
                _installmentDetailService.SaveChanges();                
            //}
            //else
            //{

            //}
            return View();
        }

    }
}