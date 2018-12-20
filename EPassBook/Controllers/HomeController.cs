using AutoMapper;
using EPassBook.DAL.DBModel;
using EPassBook.DAL.IService;
using EPassBook.Helper;
using EPassBook.Models;
using System;
using System.Linq;
using System.IO;
using System.Web;
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
        PhotoManager pm = new PhotoManager();

        public HomeController(IUserService userser, IBenificiary Ibenificiary, IMapper mapper, IInstallmentDetailService installmentDetailService)
        {
            _userser = userser;
            _Ibenificiary = Ibenificiary;
            _mapper = mapper;
            _installmentDetailService = installmentDetailService;
        }

        [CustomAuthorize(Common.Admin, Common.SiteEngineer)]
        public ActionResult Index(int? id)
        {

            Session["InstallmentId"] = id;
            string rolename = "";
            if (Session["UserDetails"] != null)
            {
                var user = Session["UserDetails"] as UserViewModel;
                var roleId = user.UserInRoles.Select(s => s.RoleId).FirstOrDefault();
                rolename = Enum.GetName(typeof(Common.WorkFlowStages), roleId);
            }
            ViewBag.RoleName = rolename;
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
        [HttpGet]
        public ActionResult Beneficiary()
        {
            return PartialView("Beneficiary", new BeneficiaryViewModel());
        }
        [Route("Beneficiary")]
        [HttpPost]
        public ActionResult Beneficiary(BeneficiaryViewModel BVM)
        {
            HttpPostedFileBase hasbandphoto = Request.Files["imgupload1"];
            HttpPostedFileBase wifephoto = Request.Files["imgupload2"];
            int i = UploadImageInDataBase(hasbandphoto, wifephoto, BVM);
            if (i==1)
            {
                return RedirectToAction("Index");
            }
            return View(BVM);
            
           
        }
       // private readonly DBContext db = new DBContext();
        public int UploadImageInDataBase(HttpPostedFileBase hphoto,HttpPostedFileBase wphoto, BeneficiaryViewModel BVM)
        {
                BVM.Hasband_Photo = pm.ConvertToBytes(hphoto);
                BVM.Wife_Photo = pm.ConvertToBytes(wphoto);
                var user = Session["UserDetails"] as UserViewModel;
                var insertbeneficiary = new BenificiaryMaster();
                //var benficiarymodel = _mapper.Map<BeneficiaryViewModel, BenificiaryMaster>(BVM);
                insertbeneficiary.Hasband_Photo = BVM.Hasband_Photo;
                insertbeneficiary.Wife_Photo = BVM.Wife_Photo;
                insertbeneficiary.BeneficairyName = BVM.BeneficairyName;
                insertbeneficiary.FatherName = BVM.FatherName;
                insertbeneficiary.Mother = BVM.Mother;
                insertbeneficiary.MobileNo = BVM.MobileNo;
                insertbeneficiary.CityId = BVM.CityId;
                insertbeneficiary.DTRNo = BVM.DTRNo;
                insertbeneficiary.RecordNo = BVM.RecordNo;
                insertbeneficiary.Class = BVM.Class;
                insertbeneficiary.General = BVM.General;
                insertbeneficiary.Single = BVM.Single;
                insertbeneficiary.Disabled = BVM.Disabled;
                insertbeneficiary.Password = BVM.Password;
                insertbeneficiary.AdharNo = BVM.AdharNo;
                insertbeneficiary.VoterID = BVM.VoterID;
                insertbeneficiary.Area = BVM.Area;
                insertbeneficiary.MojaNo = BVM.MojaNo;
                insertbeneficiary.KhataNo = BVM.KhataNo;
                insertbeneficiary.KhasraNo = BVM.KhasraNo;
                insertbeneficiary.PlotNo = BVM.PlotNo;
                insertbeneficiary.PoliceStation = BVM.PoliceStation;
                insertbeneficiary.WardNo = BVM.WardNo;
                insertbeneficiary.District = BVM.District;
                insertbeneficiary.BankName = BVM.BankName;
                insertbeneficiary.BranchName = BVM.BranchName;
                insertbeneficiary.IFSCCode = BVM.IFSCCode;
                insertbeneficiary.AccountNo = BVM.AccountNo;
                insertbeneficiary.CreatedBy = user.UserName;
                insertbeneficiary.CreatedDate = DateTime.Now;
                insertbeneficiary.CompanyID = user.CompanyID;
                _Ibenificiary.Add(insertbeneficiary);
                _Ibenificiary.SaveChanges();
                return 1;
            
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
            int installmentid = 0;
            if (Session["InstallmentId"] != null)
            {
                installmentid = Convert.ToInt32(Session["InstallmentId"]);
            }
            InstallmentDetailsViewModel installvm = new InstallmentDetailsViewModel();
            var installment = _installmentDetailService.GetInstallmentDetailById(installmentid);
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

                Session["InstallmentId"] = null;
            }
            return View();
        }

    }
}